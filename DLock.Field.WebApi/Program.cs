using DLock.Field.WebApi.Persistence;
using DLock.Field.WebApi.Services;
using DLock.Models;
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

services.AddScoped<IFieldService, FieldService>();

var app = builder.Build();

using (var initScope = app.Services.CreateScope())
{
    var provider = initScope.ServiceProvider;

    var dbContext = provider.GetRequiredService<FieldContext>();

    await dbContext.Database.MigrateAsync();

    await ResetData(dbContext);
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
    [FromServices] IFieldService fieldService) =>
{
    await fieldService.ChangeMaintenanceStatus(name, value);

    return Results.NoContent();
});

app.MapPost("api/restart", async (
    [FromServices] FieldContext context) =>
{
    await ResetData(context);

    return Results.NoContent();
});

#endregion

app.Run();

static async Task ResetData(FieldContext dbContext)
{
    await dbContext.Database.ExecuteSqlInterpolatedAsync($"UPDATE [Field] SET IsUnderMaintenance = 1, MaintenanceTime = NULL");
}