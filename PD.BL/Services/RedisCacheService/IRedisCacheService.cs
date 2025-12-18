using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.BL.Services.RedisCacheService
{
    public interface IRedisCacheService
    {
        T? GetData<T>(string key) where T : class;
        void SetData<T>(string key, T data, TimeSpan? expiration =null) where T : class;
        void RemoveData(string key);
    }
}
