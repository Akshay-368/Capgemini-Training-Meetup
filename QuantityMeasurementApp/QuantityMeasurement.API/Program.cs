using QuantityMeasurement.Application.Interfaces;
using QuantityMeasurement.Application.Services;
using QuantityMeasurement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore ;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using QuantityMeasurement.Infrastructure.Persistence;
// This file contains the code for the Dependency Injection
var builder = WebApplication.CreateBuilder(args);

var redisConfig = builder.Configuration.GetSection("Redis").GetValue<string>("Configuration") ?? "localhost:6379";
var instanceName = builder.Configuration.GetSection("Redis").GetValue<string>("InstanceName") ?? "QuantityMeasurementApp:";
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = redisConfig ; options.InstanceName = instanceName; });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register services
builder.Services.AddScoped<IQuantityService, QuantityService>();

// register the Application->Infrastructure repository implementation
builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();

// db context

builder.Services.AddDbContext<QuantityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();