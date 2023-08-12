using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using System.Collections;
using System.Text;
using MiComunidadPro.Common.Settings;
using Microsoft.Extensions.Caching.Memory;
using MiComunidadPro.Common.Contracts;

namespace MiComunidadPro.Web.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly string _EnvironmentName;
        private readonly IMemoryCache _Cache;
        private readonly CacheSettings _CacheSettings;
        // private readonly JsonOptions _JsonOptions;

        public CacheService(IMemoryCache cache, CacheSettings settings, 
                            // JsonOptions jsonOptions, 
                            IWebHostEnvironment hostingEnvironment)
        {
            _Cache = cache;
            _CacheSettings = settings;
            // _JsonOptions = jsonOptions;
            _EnvironmentName = hostingEnvironment.EnvironmentName;
        }

        public void AppendToCachedList<T>(string key, T item)
        {
            if(!_CacheSettings.Enabled)
                return;

            var obj = _Cache.Get(key);

            if(obj is not null)
            {
                var list = obj as List<T>;

                list.Add(item);
            }
        }

        public T GetCached<T>(string key)
        {
            var result = default(T);

            if(!_CacheSettings.Enabled)
                return result;

            var output = _Cache.Get<T>($"{_EnvironmentName}{key}");
            
            if(output is not null)
            {
                return output;
            }

            //Nothing was found
            return default(T);
        }

        public async Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer, TimeSpan slidingExpiration)
        {
            var result = default(T);

            if(!_CacheSettings.Enabled)
            {
                result = await initializer();

                return result;
            }
            var finalKey = $"{_EnvironmentName}{key}";

            var chacheData = _Cache.Get<T>(finalKey);

            if(chacheData is null)
            {
                var options = new MemoryCacheEntryOptions();

                options.SetAbsoluteExpiration(slidingExpiration);

                result = await initializer();

                _Cache.Set(finalKey, chacheData, options);
            }
            else
            {
                result = chacheData;
            }

            // taking care of value types
            if(result == null && (typeof(T)).IsValueType)
            {
                return default(T);
            }

            return result;
        }

        public async Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer, string timeoutKey = "*")
        {
            var timeoutSetting = _CacheSettings.ExpirationTimes.FirstOrDefault(t => t.Key.ToLower().Equals(timeoutKey.ToLower()));

            if(timeoutSetting is null)
                timeoutSetting = new ExpirationTime
                {
                    Duration = 60000
                };

            int duration = timeoutSetting.Duration;
            return await GetCachedAsync($"{_EnvironmentName}{key}", initializer, TimeSpan.FromMilliseconds(duration));
        }

        public async Task<T> GetCachedAsync<T>(string key, Func<Task<T>> initializer)
        {
            return await GetCachedAsync($"{_EnvironmentName}{key}", initializer,  "*");
        }

        public void Clear(string key)
        {
            if(_CacheSettings.Enabled)
                _Cache.Remove($"{_EnvironmentName}{key}");
        }
    }
}
