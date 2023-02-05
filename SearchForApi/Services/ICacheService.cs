using System;
using System.Threading.Tasks;

namespace SearchForApi.Services
{
    public interface ICacheService
    {
         Task SetItem<T>(string key, T value, DateTime expireDate);
         Task SetItem<T>(string key, T value, TimeSpan expireDate);
         Task<T> GetItem<T>(string key);
         Task RemoveItem(string key);
    }
}