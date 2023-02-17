using DLock.Identity.WebApi.Models;
using DLock.Identity.WebApi.Persistence;
using DLock.Identity.WebApi.Services;
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
    .AddDbContext<IdentityContext>(opt =>
    {
        opt.UseSqlServer(configuration.GetConnectionString(nameof(IdentityContext)));
    });

services.AddScoped<IUserService, UserService>();

var app = builder.Build();

using (var initScope = app.Services.CreateScope())
{
    var provider = initScope.ServiceProvider;

    var dbContext = provider.GetRequiredService<IdentityContext>();

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

app.MapGet("api/users", async ([FromServices] IUserService userService) =>
{
    var users = await userService.GetAllUsers();

    return Results.Ok(new ApiResponse
    {
        Data = users
    });
});

var loginLock = new SemaphoreSlim(1, 1);

app.MapPost("api/login", async ([FromBody] LoginModel model, [FromServices] IUserService userService) =>
{
    if (!loginLock.Wait(7000))
    {
        return Results.BadRequest(new ApiResponse
        {
            Messages = new[] { "Resource is busy!" }
        });
    }

    var user = await userService.FindUser(model.UserName);

    if (user != null)
    {
        return Results.BadRequest(new ApiResponse
        {
            Messages = new[] { "User already exists!" }
        });
    }

    await userService.CreateUser(model.UserName);

    loginLock.Release();

    return Results.NoContent();
});

app.MapPost("api/logout/{userName}", async ([FromRoute] string userName, [FromServices] IUserService userService) =>
{
    var user = await userService.FindUser(userName);

    if (user != null)
    {
        await userService.RemoveUser(userName);
    }

    return Results.NoContent();
});

app.MapPost("api/restart", async ([FromServices] IdentityContext context) =>
{
    await ResetData(context);

    return Results.NoContent();
});

#endregion

app.Run();

static async Task ResetData(IdentityContext dbContext)
{
    await dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM [User];");
}