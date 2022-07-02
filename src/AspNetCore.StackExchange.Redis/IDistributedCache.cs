namespace AspNetCore.StackExchange.Redis;

public interface IDistributedCache: IDisposable
{
     /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<object> GetAsync(string key);

        /// <summary>
        /// Get Cache Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Get Cache Item Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="acquire"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        T Get<T>(string key, Func<T> acquire, TimeSpan? expire = null);

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it with Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="acquire"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key, Func<T> acquire, TimeSpan? expire = null);

        /// <summary>
        /// Add the specified key and object to the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        bool Set(string key, object data, TimeSpan? expire = null);

        /// <summary>
        /// Add the specified key and object to the cache with Async
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, object data, TimeSpan? expire = null);

        /// <summary>
        /// Get Hash Item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        T GetHashItem<T>(string key, string hashKey);

        /// <summary>
        /// Get Hash Item With Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        Task<T> GetHashItemAsync<T>(string key, string hashKey);

        /// <summary>
        /// Get Hash Items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<object, T> GetHashItems<T>(string key);

        /// <summary>
        /// Get Hash Items with Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Dictionary<object, T>> GetHashItemsAsync<T>(string key);

        /// <summary>
        /// Set HashItem to cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool SetToHashItem(string key, string hashKey, object data);

        /// <summary>
        /// Set HashItem to cache with Async
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> SetToHashItemAsync(string key, string hashKey, object data);

        /// <summary>
        /// PerformActionWithLock
        /// </summary>
        /// <param name="lockKey"></param>
        /// <param name="lockValue"></param>
        /// <param name="expire"></param>
        /// <param name="action"></param>
        void PerformActionWithLock(string lockKey, string lockValue, TimeSpan expire, Action action);

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(string key);

        /// <summary>
        /// Removes the value with the specified key from the cache with Async
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key);
}