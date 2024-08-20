using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace RedisDemo3.Cache
{
    public class CacheService : ICacheService
    {
        private IDistributedCache _cacheService;

        public CacheService(IDistributedCache cacheService)
        {
            _cacheService = cacheService;
        }

        public T GetData<T>(string key)
        {
            var value = _cacheService.GetString(key);
            if(!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        public object RemoveData(string key)
        {
            _cacheService.Remove(key);
            return true;
        }

        /// <summary>
        /// Set cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime">Time when cache expired</param>
        public void SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expirationTime,
            };
            _cacheService.SetString(key, JsonConvert.SerializeObject(value), options);
        }
    }
}
