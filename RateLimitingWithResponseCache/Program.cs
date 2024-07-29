using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using RateLimitingWithResponseCache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting(); // Register in-memory rate limiting services

// Load specific IpRateLimit configuration
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimiting:Policies"));

// Register rate limit configuration
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();


CacheHelper.BuildCache();
// Configure the HTTP request pipeline.
app.UseIpRateLimiting();
app.UseResponseCaching();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
