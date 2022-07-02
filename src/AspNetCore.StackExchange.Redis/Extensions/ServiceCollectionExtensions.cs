using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore.StackExchange.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStackExchangeRedisCache(this IServiceCollection services, Func<RedisConfiguration> configuration)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));
        services.AddSingleton(x => new RedisConnection(configuration.Invoke(), x.GetRequiredService<ILogger<RedisConnection>>()));
        services.AddSingleton<IDistributedCache, DistributedCache>();
        services.AddSingleton<ISerializer, Serializer>();
        return services;
    }
}