using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AspNetCore.StackExchange.Redis;

public class RedisConnection
{
    private static readonly Lazy<ConfigurationOptions> Configuration = new(() => _redisConfiguration.ConfigurationOptions);

    private static readonly Lazy<ConnectionMultiplexer> Connection = new(() => ConnectionMultiplexer.Connect(Configuration.Value));

    private static RedisConfiguration _redisConfiguration;
    private readonly ILogger<RedisConnection> _logger;

    public RedisConnection(RedisConfiguration redisConfiguration, ILogger<RedisConnection> logger)
    {
        _redisConfiguration = redisConfiguration;
        _logger = logger;
    }

    public IDatabase GetDatabase()
    {
        return GetConnection().GetDatabase(_redisConfiguration.Database);
    }

    private ConnectionMultiplexer GetConnection()
    {
        Connection.Value.ConnectionFailed += ValueOnConnectionFailed;
        Connection.Value.ConnectionRestored += ValueOnConnectionRestored;

        return Connection.Value;
    }

    private void ValueOnConnectionFailed(object? sender, ConnectionFailedEventArgs e)
    {
        _logger.LogError(e.Exception,"[RedisConnectionFailed]");
    }
    
    private void ValueOnConnectionRestored(object? sender, ConnectionFailedEventArgs e)
    {
        _logger.LogError(e.Exception,"[RedisConnectionRestored]");
    }
}