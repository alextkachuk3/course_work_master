using MathService.ApiService.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisDistributedCache("cache");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<MatrixService>();
builder.Services.AddScoped<RedisCacheService>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.Use(async (context, next) =>
{
    await next();

    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        context.Response.Headers.Remove("Cache-Control");
        context.Response.Headers.Remove("Pragma");
        context.Response.Headers.Remove("Expires");
        context.Response.Headers["Cache-Control"] = "public, max-age=3600";
    }
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async (IDistributedCache cache) =>
{
    var cachedForecast = await cache.GetAsync("forecast");

    if (cachedForecast is null)
    {
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

        await cache.SetAsync("forecast", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(forecast)), new()
        {
            AbsoluteExpiration = DateTime.Now.AddSeconds(10)
        }); ;

        return forecast;
    }

    return JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(cachedForecast);
})
.WithName("GetWeatherForecast");

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
