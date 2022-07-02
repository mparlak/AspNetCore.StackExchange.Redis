using StackExchange.Redis;

namespace AspNetCore.StackExchange.Redis;

public interface IRedisConnection
{
    IDatabase GetDatabase();
}