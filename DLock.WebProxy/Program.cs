using DLock.Models;
using DLock.Services;
using DLock.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddCors()
    .AddRedisConnection(configuration)
    .SetupReverseProxy(builder.Configuration);

services
    .AddSingleton<ILockManager, LockManager>()
    .AddSingleton<SingleRedisLockService>()
    .AddSingleton<RedLockService>();

var app = builder.Build();

using (var initScope = app.Services.CreateScope())
{
    var provider = initScope.ServiceProvider;

    var redLockService = provider.GetRequiredService<RedLockService>();
    var multiplexer = provider.GetRequiredService<ConnectionMultiplexer>();

    await ResetData(redLockService);

    await InitializeDatabase(app.Configuration, multiplexer);
}

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapPost("sharing", (
    [FromBody] string data,
    [FromServices] ConnectionMultiplexer connection) =>
{
    var db = connection.GetDatabase();

    db.StringSet("dlock-sharing", data);

    return Results.NoContent();
});

app.MapGet("sharing", ([FromServices] ConnectionMultiplexer connection) =>
{
    var db = connection.GetDatabase();

    var sharingData = (string)db.StringGet("dlock-sharing");

    return Results.Ok(new ApiResponse
    {
        Data = sharingData,
    });
});

app.Map("config/{configName}", (
    [FromRoute] string configName,
    [FromServices] InMemoryConfigProvider configProvider) =>
{
    var currentConfig = configProvider.GetConfig();
    var routes = currentConfig.Routes;
    configProvider.Update(routes, _clusterConfigMap[configName]);
    return Results.NoContent();
});

app.MapPut("lock/{serviceName}", (
    [FromRoute] string serviceName,
    [FromServices] ILockManager lockManager) =>
{
    lockManager.SetLockService(serviceName);

    return Results.NoContent();
});

app.MapReverseProxy();

app.Run();

static async Task ResetData(RedLockService redLockService)
{
    await redLockService.ResetData();
}

static async Task InitializeDatabase(
    IConfiguration configuration,
    ConnectionMultiplexer multiplexer)
{
    const string InitKey = "dlock-init";
    var db = multiplexer.GetDatabase();

    if (db.KeyExists(InitKey))
    {
        return;
    }
    else
    {
        db.StringSet(InitKey, 1);
    }

    var connStr = configuration.GetConnectionString("Default");

    string createDbCmd = $@"
USE [master]
CREATE DATABASE [DLock_HandsOn];
";

    string createTableCmd = $@"
USE [DLock_HandsOn];
CREATE TABLE [Resources] (
	Name nvarchar(256) PRIMARY KEY,
	Value nvarchar(max) NOT NULL
);
";

    string createLoginCmd = $@"
CREATE LOGIN [readonly]  
    WITH PASSWORD = '123456',
	CHECK_POLICY = OFF;";

    string createUserCmd = $@"CREATE USER [readonly] FOR LOGIN [readonly];";

    string assignRoleCmd = $@"EXEC sp_addrolemember 'db_datareader', 'readonly';";

    var listCmds = new[] { createDbCmd, createTableCmd, createLoginCmd, createUserCmd, assignRoleCmd };

    using (SqlConnection connection = new SqlConnection(connStr))
    {
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        connection.Open();

        foreach (var cmd in listCmds)
        {
            command.CommandText = cmd;
            await command.ExecuteNonQueryAsync();
        }

        for (var i = 1; i <= 5; i++)
        {
            command.CommandText = @"INSERT INTO [Resources] VALUES(@Name, @Value);";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("Name", $"Sample Key {i}");
            command.Parameters.AddWithValue("Value", $"Sample Value {i}");
            await command.ExecuteNonQueryAsync();
        }
    }
}

class ProxyConfig
{
    public Dictionary<string, RouteConfig> Routes { get; set; }
    public Dictionary<string, ClusterConfig> Clusters { get; set; }
}

public static partial class Program
{
    public const string SingleServerConfigName = "SingleServer";
    public const string MultipleServerConfigName = "MultipleServer";

    static Program()
    {
        _clusterConfigMap = new Dictionary<string, IReadOnlyList<ClusterConfig>>();
    }

    private static readonly Dictionary<string, IReadOnlyList<ClusterConfig>> _clusterConfigMap;

    public static IServiceCollection SetupReverseProxy(this IServiceCollection services, IConfiguration configuration)
    {
        var proxyConfig = new ProxyConfig();
        configuration.Bind("ReverseProxy", proxyConfig);

        var routes = proxyConfig.Routes.Select(kvp =>
        {
            var routeId = kvp.Key;
            var route = kvp.Value;
            return new RouteConfig
            {
                ClusterId = route.ClusterId,
                RouteId = routeId,
                Match = route.Match,
                Transforms = route.Transforms
            };
        }).ToList();

        var clusters = proxyConfig.Clusters.Select(kvp =>
        {
            var clusterId = kvp.Key;
            var cluster = kvp.Value;
            return new ClusterConfig
            {
                ClusterId = clusterId,
                Destinations = cluster.Destinations,
                LoadBalancingPolicy = cluster.LoadBalancingPolicy,
            };
        }).ToList();

        InitializeClusterConfigMap(clusters);

        services.AddReverseProxy()
            .LoadFromMemory(routes, _clusterConfigMap[SingleServerConfigName]);

        return services;
    }

    public static void InitializeClusterConfigMap(IReadOnlyList<ClusterConfig> clusters)
    {
        _clusterConfigMap[MultipleServerConfigName] = clusters;
        var bookingCluster = clusters.Where(c => c.ClusterId == "Booking").FirstOrDefault();

        if (bookingCluster != null)
        {
            bookingCluster = new ClusterConfig
            {
                Destinations = bookingCluster.Destinations.Take(1).ToDictionary(o => o.Key, o => o.Value),
                ClusterId = bookingCluster.ClusterId,
                LoadBalancingPolicy = bookingCluster.LoadBalancingPolicy
            };

            _clusterConfigMap[SingleServerConfigName] = new[] { bookingCluster };
        }
    }
}