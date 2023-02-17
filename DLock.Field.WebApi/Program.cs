using DLock.Field.WebApi.Persistence;
using DLock.Field.WebApi.Services;
using DLock.Models;
using DLock.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCors()
    .AddDbContext<FieldContext>(opt =>
    {
        opt.UseSqlServer(configuration.GetConnectionString(nameof(FieldContext)));
    });

services.AddScoped<IFieldService, FieldService>()
    .AddSingleton<ILockManager, LockManager>()
    .AddSingleton<DistributedLockService>();

var app = builder.Build();

using (var initScope = app.Services.CreateScope())
{
    var provider = initScope.ServiceProvider;

    var dbContext = provider.GetRequiredService<FieldContext>();
    var lockManager = provider.GetRequiredService<ILockManager>();
    var distributedLockService = provider.GetRequiredService<DistributedLockService>();

    await dbContext.Database.MigrateAsync();

    await ResetData(dbContext, lockManager, distributedLockService);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

#region API endpoints

app.MapGet("api/fields", async ([FromServices] IFieldService fieldService) =>
{
    var fields = await fieldService.GetAllFields();

    return Results.Ok(new ApiResponse { Data = fields });
});

app.MapGet("api/fields/{name}", async ([FromRoute] string name, [FromServices] IFieldService fieldService) =>
{
    var field = await fieldService.FindField(name);

    if (field == null) return Results.NotFound();

    return Results.Ok(new ApiResponse { Data = field });
});

app.MapPut("api/fields/maintenance/{value}", async (
    [FromRoute] bool value,
    [FromQuery] string name,
    [FromServices] IFieldService fieldService,
    [FromServices] ILockManager lockManager) =>
{
    ILock lockObj = null;

    if (!string.IsNullOrWhiteSpace(name))
    {
        var lockService = lockManager.GetLockService();
        var lockKey = $"field-{name}";
        lockObj = await lockService.AcquireLock(lockKey);

        if (lockObj == null)
        {
            return Results.BadRequest(new ApiResponse
            {
                Messages = new[] { $"{name} is busy" }
            });
        }
    }

    await fieldService.ChangeMaintenanceStatus(name, value);

    if (lockObj != null)
    {
        await lockObj.ReleaseLock();
    }

    return Results.NoContent();
});

app.MapPost("api/restart", async (
    [FromServices] FieldContext context,
    [FromServices] ILockManager lockManager,
    [FromServices] DistributedLockService distributedLockService) =>
{
    await ResetData(context, lockManager, distributedLockService);

    return Results.NoContent();
});

app.MapPut("lock/{serviceName}", (
    [FromRoute] string serviceName,
    [FromServices] ILockManager lockManager) =>
{
    lockManager.SetLockService(serviceName);

    return Results.NoContent();
});

#endregion

app.Run();

static async Task ResetData(FieldContext dbContext,
    ILockManager lockManager,
    DistributedLockService distributedLockService)
{
    lockManager.Reset();

    await distributedLockService.ResetData();

    await dbContext.Database.ExecuteSqlInterpolatedAsync($"UPDATE [Field] SET IsUnderMaintenance = 1, MaintenanceTime = NULL");
}