using System;
using System.Threading.Tasks;

namespace MiComunidadPro.Common.Contracts
{
    public interface ICacheService
    {
        void AppendToCachedList<T>(string key, T item);
        T GetCached<T>(string key);

        Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer);

        Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer, string timeoutKey = "*");

        Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer, TimeSpan slidingExpiration);

        void Clear(string key);
    }
}