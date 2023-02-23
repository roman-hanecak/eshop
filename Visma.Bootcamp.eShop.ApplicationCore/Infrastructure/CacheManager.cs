using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.eShop.ApplicationCore.Infrastructure
{
    public class CacheManager : ICacheManager
    {
        private readonly IDictionary<Type, string> _domainTypes;
        private readonly IMemoryCache _cache;

        public CacheManager(IMemoryCache cache)
        {
            _cache = cache;
            _domainTypes = new Dictionary<Type, string>
            {
                { typeof(BasketDto), "Baskets" }
            };
        }

        public void Set<T>(T item) where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            if (_cache.TryGetValue(category, out List<T> cachedItems))
            {
                var cachedItem = cachedItems.SingleOrDefault(x => x.Id == item.Id);
                if (cachedItem != null)
                {
                    cachedItems.Remove(cachedItem);
                }

                cachedItems.Add(item);
            }
            else
            {
                cachedItems = new List<T> { item };
            }

            _cache.Set(category, cachedItems, TimeSpan.FromHours(24));
        }

        public List<T> Get<T>() where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            return _cache.GetOrCreate(category, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
                return new List<T>();
            });
        }

        public T Get<T>(Guid itemId) where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            List<T> items = Get<T>(); // get all items for given category
            return items.SingleOrDefault(x => x.Id == itemId);
        }

        public void Remove<T>(Guid itemId) where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            List<T> cachedItems = Get<T>(); // get all items for given category
            var item = cachedItems.SingleOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                cachedItems.Remove(item);
                _cache.Set(category, cachedItems, TimeSpan.FromHours(24));
            }
        }

        private string GetCategoryOrThrow(Type itemType)
        {
            return !_domainTypes.ContainsKey(itemType)
                ? throw new Exception("Unsupported type of cacheable object")
                : _domainTypes[itemType];
        }

        public Task SetAsync<T>(T item, CancellationToken ct = default) where T : ICacheableDto
        {
            Set<T>(item);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAsync<T>(CancellationToken ct = default) where T : ICacheableDto
        {
            return await Task.FromResult(Get<T>());
        }

        public async Task<T> GetAsync<T>(Guid itemId, CancellationToken ct = default) where T : ICacheableDto
        {
            return await Task.FromResult(Get<T>(itemId));
        }

        public Task RemoveAsync<T>(Guid itemId, CancellationToken ct = default) where T : ICacheableDto
        {
            Remove<T>(itemId);
            return Task.CompletedTask;
        }
    }
}
