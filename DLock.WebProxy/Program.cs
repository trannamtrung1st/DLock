using DLock.Models;
using DLock.Services;
using DLock.Utils;
using Microsoft.AspNetCore.Mvc;
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

    await ResetData(redLockService);
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