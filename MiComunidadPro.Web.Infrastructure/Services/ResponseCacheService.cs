using MiComunidadPro.Common.Settings;
using MiComunidadPro.Web.Infrastructure.Contracts;
using MiComunidadPro.Web.Infrastructure.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IMemoryCache _Cache;
        private readonly CacheSettings _Settings;

        public ResponseCacheService(IMemoryCache cache, CacheSettings _settings)
        {
            _Cache = cache;
            _Settings = _settings;
        }

        public void CacheResponse(string cacheKey, object response, TimeSpan timeToLive)
        {
            if(response == null || !_Settings.Enabled)
                return;

            var serializeResponse = JsonConvert.SerializeObject(response);

            var options = new MemoryCacheEntryOptions();
            options.SetAbsoluteExpiration(timeToLive);

            _Cache.Set(cacheKey, serializeResponse, options);
        }

        public string GetCachedResponse(string cacheKey)
        {
            if(!_Settings.Enabled)
                return null;

            var cachedResponse = _Cache.Get(cacheKey);
            return cachedResponse is null ? null : cachedResponse.ToString();
        }
    }
}
