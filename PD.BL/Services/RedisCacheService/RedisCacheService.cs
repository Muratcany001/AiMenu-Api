using Common.ViewModels;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.RedisCacheService
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public T? GetData<T>(string key) where T : class
        {
            var data = _cache.GetString(key);
            if (data == null)
            {
                return null;
            }
            return System.Text.Json.JsonSerializer.Deserialize<T>(data);
        }

        public void RemoveData(string key)
        {
            var data = _cache.GetString(key);
            if (data != null)
            {
                _cache.Remove(key);
            }
            
        }

        public void SetData<T>(string key, T data, TimeSpan? expiration = null) where T : class
        {
            var option = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
            };
            _cache.SetString(key, System.Text.Json.JsonSerializer.Serialize(data), option);

        }
    }
}
