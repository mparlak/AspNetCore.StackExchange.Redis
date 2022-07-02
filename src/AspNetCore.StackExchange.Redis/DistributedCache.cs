using StackExchange.Redis;

namespace AspNetCore.StackExchange.Redis;

public class DistributedCache : IDistributedCache
{
    private readonly IDatabase _db;
    private readonly ISerializer _serializer;

    public DistributedCache(RedisConnection redisConnection, ISerializer serializer)
    {
        _db = redisConnection.GetDatabase();
        _serializer = serializer;
    }

    public object Get(string key)
    {
        var cacheItem = _db.StringGet(key);

        return !cacheItem.HasValue ? default(object) : cacheItem;
    }

    public async Task<object> GetAsync(string key)
    {
        var cacheItem = await _db.StringGetAsync(key);

        return !cacheItem.HasValue ? default(object) : cacheItem;
    }

    public T Get<T>(string key)
    {
        var cacheItem = _db.StringGet(key);

        if (!cacheItem.HasValue)
        {
            return default;
        }

        var result = _serializer.Deserialize<T>(cacheItem);

        return result != null ? result : default;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var cacheItem = await _db.StringGetAsync(key);

        if (!cacheItem.HasValue)
        {
            return default;
        }

        var result = _serializer.Deserialize<T>(cacheItem);

        return result != null ? result : default;
    }

    public T Get<T>(string key, Func<T> acquire, TimeSpan? expire = null)
    {
        var cacheItem = Get<T>(key);

        if (cacheItem != null)
        {
            return cacheItem;
        }

        var result = acquire();

        Set(key, result, expire);

        return result;
    }

    public async Task<T> GetAsync<T>(string key, Func<T> acquire, TimeSpan? expire = null)
    {
        var cacheItem = await GetAsync<T>(key);

        if (cacheItem != null)
        {
            return cacheItem;
        }

        var result = acquire();

        await SetAsync(key, result, expire);

        return result;
    }

    public bool Set(string key, object data, TimeSpan? expire = null)
    {
        if (data == null)
        {
            return false;
        }

        var cacheItem = _serializer.Serialize(data);

        return _db.StringSet(key, cacheItem, expire);
    }

    public async Task<bool> SetAsync(string key, object data, TimeSpan? expire = null)
    {
        if (data == null)
        {
            return false;
        }

        var cacheItem = _serializer.Serialize(data);

        return await _db.StringSetAsync(key, cacheItem, expire);
    }

    public T GetHashItem<T>(string key, string hashKey)
    {
        var cacheItem = _db.HashGet(key, hashKey);

        if (!cacheItem.HasValue)
        {
            return default;
        }

        var result = _serializer.Deserialize<T>(cacheItem);

        return result ?? default;
    }

    public async Task<T> GetHashItemAsync<T>(string key, string hashKey)
    {
        var cacheItem = await _db.HashGetAsync(key, hashKey);

        if (!cacheItem.HasValue)
        {
            return default;
        }

        var result = _serializer.Deserialize<T>(cacheItem);

        return result ?? default;
    }

    public Dictionary<object, T> GetHashItems<T>(string key)
    {
        var cacheItems = _db.HashGetAll(key);

        if (cacheItems?.Length > default(int))
        {
            var items = new Dictionary<object, T>();

            foreach (var cacheItem in cacheItems)
            {
                if (!items.ContainsKey(cacheItem.Key))
                {
                    items.Add(cacheItem.Key, _serializer.Deserialize<T>(cacheItem.Value));
                }
            }

            return items;
        }

        return default;
    }

    public async Task<Dictionary<object, T>> GetHashItemsAsync<T>(string key)
    {
        var cacheItems = await _db.HashGetAllAsync(key);

        if (cacheItems?.Length > default(int))
        {
            var items = new Dictionary<object, T>();

            foreach (var cacheItem in cacheItems)
            {
                if (!items.ContainsKey(cacheItem.Key))
                {
                    items.Add(cacheItem.Key, _serializer.Deserialize<T>(cacheItem.Value));
                }
            }

            return items;
        }

        return default;
    }

    public bool SetToHashItem(string key, string hashKey, object data)
    {
        if (data == null)
        {
            return false;
        }

        var cacheItem = _serializer.Serialize(data);

        return _db.HashSet(key, hashKey, cacheItem, When.NotExists);
    }

    public async Task<bool> SetToHashItemAsync(string key, string hashKey, object data)
    {
        if (data == null)
        {
            return false;
        }

        var cacheItem = _serializer.Serialize(data);

        return await _db.HashSetAsync(key, hashKey, cacheItem, When.NotExists);
    }

    public void PerformActionWithLock(string lockKey, string lockValue, TimeSpan expire, Action action)
    {
        if (_db.LockTake(lockKey, lockValue, expire))
        {
            action();
        }
    }

    public bool Remove(string key)
    {
        return _db.KeyDelete(key);
    }

    public async Task<bool> RemoveAsync(string key)
    {
        return await _db.KeyDeleteAsync(key);
    }

    public void Dispose()
    {
    }
}