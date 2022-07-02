using AspNetCore.StackExchange.Redis;
using AspNetCore.StackExchange.Redis.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((context, config) =>
{
    var env = context.HostingEnvironment;
    config.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
});

builder.Host.UseSerilog((_, config) =>
{
    config
        .WriteTo.Console()
        .WriteTo.Elasticsearch()
        .ReadFrom.Configuration(builder.Configuration);
});


builder.Services.AddStackExchangeRedisCache(() =>
{
    var redisHosts = builder.Configuration.GetSection("RedisCacheConfig:RedisHosts").Get<RedisHost[]>();
    var password = builder.Configuration.GetValue<string>("RedisCacheConfig:Password");
    //var database = builder.Configuration.GetValue<int>("RedisCacheConfig:Password");
    return new RedisConfiguration
    {
        //Database = database,
        Hosts = redisHosts,
        Password = password,
        AllowAdmin = true
    };
});

var app = builder.Build();

app.MapPost("/v1/redis", async (IDistributedCache cache) =>
{
    await cache.SetAsync("sample:key:1", new RedisConfiguration
    {
        Database = 0,
        Hosts = new[]
        {
            new RedisHost
            {
                Host = "localhost",
                Port = 6379
            }
        },
        Password = "123"
    }, TimeSpan.FromMinutes(30));

    return Results.Created("", true);
});

app.MapGet("/", () => "Hello World!");

app.Run();