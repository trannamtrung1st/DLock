using DLock.Booking.WebApi.Models;
using DLock.Booking.WebApi.Persistence;
using DLock.Booking.WebApi.Services;
using DLock.Models;
using DLock.Services;
using DLock.Utils;
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
    .AddRedisConnection(configuration)
    .AddDbContext<BookingContext>(opt =>
    {
        opt.UseSqlServer(configuration.GetConnectionString(nameof(BookingContext)));
    });

services.AddHttpClient("FieldApi", client =>
{
    client.BaseAddress = new Uri(configuration["FieldApiBaseUrl"]);
});

services.AddScoped<IBookingService, BookingService>()
    .AddSingleton<ILockManager, LockManager>()
    .AddSingleton<SingleRedisLockService>()
    .AddSingleton<RedLockService>();

var app = builder.Build();

if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("SHOULD_INIT") == "1")
{
    using (var initScope = app.Services.CreateScope())
    {
        var provider = initScope.ServiceProvider;

        var dbContext = provider.GetRequiredService<BookingContext>();

        await dbContext.Database.MigrateAsync();

        await ResetData(dbContext);
    }
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

app.MapGet("server", () => Results.Ok($"Server: {ServerId}"));

app.MapGet("api/bookings", async (
    [FromServices] IBookingService bookingService) =>
{
    var bookings = await bookingService.GetBookings();

    return Results.Ok(new ApiResponse
    {
        Data = bookings
    });
});

app.MapGet("api/bookings/{userName}", async (
    [FromRoute] string userName,
    [FromServices] IBookingService bookingService) =>
{
    var bookings = await bookingService.GetBookings(userName);

    return Results.Ok(new ApiResponse
    {
        Data = bookings
    });
});

app.MapPost("api/bookings", async (
    [FromBody] CreateBookingModel model,
    [FromServices] IBookingService bookingService,
    [FromServices] IHttpClientFactory clientFactory,
    [FromServices] ILockManager lockManager) =>
{
    var lockService = lockManager.GetLockService();
    var lockKey = $"field-{model.FieldName}";
    var lockObj = await lockService.AcquireLock(lockKey);

    if (lockObj == null)
    {
        return Results.BadRequest(new ApiResponse
        {
            Messages = new[] { $"{model.FieldName} is busy" }
        });
    }

    var fieldClient = clientFactory.CreateClient("FieldApi");

    var response = await fieldClient.GetAsync($"api/fields/{Uri.EscapeDataString(model.FieldName)}");

    if (response.IsSuccessStatusCode)
    {
        var data = await response.Content.ReadFromJsonAsync<ApiResponse<FieldModel>>();

        if (data.Data.IsUnderMaintenance)
        {
            await lockObj.ReleaseLock();

            return Results.BadRequest(new ApiResponse
            {
                Messages = new[] { $"{model.FieldName} under maintenance!" }
            });
        }
    }
    else
    {
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            await lockObj.ReleaseLock();

            var data = await response.Content.ReadFromJsonAsync<ApiResponse>();

            return Results.BadRequest(data);
        }
        else
        {
            await lockObj.ReleaseLock();

            return Results.Problem();
        }
    }

    var available = await bookingService.CheckIsFieldAvailableForBooking(model.FieldName);

    if (available)
    {
        var newBooking = await bookingService.CreateBooking(model);

        await lockObj.ReleaseLock();

        return Results.Ok(new ApiResponse
        {
            Data = newBooking
        });
    }
    else
    {
        await lockObj.ReleaseLock();

        return Results.BadRequest(new ApiResponse
        {
            Messages = new[] { $"{model.FieldName} is not available" }
        });
    }
});

app.MapPost("api/restart", async (
    [FromServices] BookingContext context) =>
{
    await ResetData(context);

    return Results.NoContent();
});

#endregion

app.Run();

static async Task ResetData(BookingContext dbContext)
{
    await dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Booking");
}

public static partial class Program
{
    public static readonly string ServerId = new Random().Next(100).ToString();
}