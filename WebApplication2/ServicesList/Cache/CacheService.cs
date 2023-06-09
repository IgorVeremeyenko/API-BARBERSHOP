using Microsoft.Extensions.Caching.Memory;

namespace WebApplication2.Services.Cache
{

    public interface ICacheService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan cacheTime);

        void Delete(string key);

        bool TryGetValueFromList<T>(string key);
        bool TryGetValueSingle<T>(string key);
    }
    public class CacheService : ICacheService
    {

        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public bool TryGetValueFromList<T>(string key)
        {
            T value = default;
            return _cache.TryGetValue(key, out value);
        }

        public bool TryGetValueSingle<T>(string key)
        {
            T value = default;
            return _cache.TryGetValue(key, out value);
        }

        public void Set<T>(string key, T value, TimeSpan cacheTime)
        {
            _cache.Set(key, value, cacheTime);
        }

        public void Delete(string key)
        {
            _cache.Remove(key);
        }
    }
}
