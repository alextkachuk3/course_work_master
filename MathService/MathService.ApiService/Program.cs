using MathService.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisDistributedCache("cache");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddScoped<MatrixService>();
builder.Services.AddScoped<FibonacciService>();
builder.Services.AddScoped<RedisCacheService>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.MapControllers();

app.Run();
