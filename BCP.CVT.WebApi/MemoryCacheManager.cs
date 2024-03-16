using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace BCP.CVT.WebApi
{
    public class MemoryCacheManager<TItem>
    {
        private MemoryCache _cache = MemoryCache.Default;

        public TItem GetOrCreate(string clave, TItem createItem)
        {
            TItem cacheEntry;

            if (_cache.Get(clave) == null)
            {                
                cacheEntry = createItem;

                var cacheItemPolicy = new CacheItemPolicy
                {
                    SlidingExpiration = TimeSpan.FromHours(8),
                    Priority = CacheItemPriority.Default
                };
                
                _cache.Set(clave, cacheEntry, cacheItemPolicy);
            }
            else
                cacheEntry = (TItem)_cache.Get(clave);

            return cacheEntry;
        }

        public TItem GetCache(string clave)
        {
            if (_cache.Get(clave) == null)
                return default(TItem);
            else
                return (TItem)_cache.Get(clave);
        }
    }
}