using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Web.Infrastructure.Contracts
{
    public interface IResponseCacheService
    {
        void CacheResponse(string cacheKey, object response, TimeSpan timeToLive);

        string GetCachedResponse(string cacheKey);
    }
}
