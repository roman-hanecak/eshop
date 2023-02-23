using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.eShop.ApplicationCore.Infrastructure
{
    public class DistributedCacheManager : ICacheManager
    {
        private readonly IDictionary<Type, string> _domainTypes;
        private readonly IDistributedCache _cache;

        public DistributedCacheManager(IDistributedCache cache)
        {
            _cache = cache;
            _domainTypes = new Dictionary<Type, string>
            {
                { typeof(BasketDto), "Baskets" }
            };
        }

        public List<T> Get<T>() where T : ICacheableDto
        {
            return GetAsync<T>().Result;
        }

        public T Get<T>(Guid itemId) where T : ICacheableDto
        {
            return GetAsync<T>(itemId).Result;
        }

        public async Task<List<T>> GetAsync<T>(CancellationToken ct = default) where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            return await ReadAsync<T>(category, ct);
        }

        public async Task<T> GetAsync<T>(Guid itemId, CancellationToken ct = default) where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            List<T> cachedItems = await ReadAsync<T>(category, ct);
            return cachedItems.SingleOrDefault(x => x.Id == itemId);
        }

        public void Remove<T>(Guid itemId) where T : ICacheableDto
        {
            RemoveAsync<T>(itemId).Wait();
        }

        public async Task RemoveAsync<T>(Guid itemId, CancellationToken ct = default) where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            List<T> cachedItems = await ReadAsync<T>(category, ct);
            var item = cachedItems.SingleOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                cachedItems.Remove(item);
                await SerializeToCache<T>(category, cachedItems, ct);
            }
        }

        public void Set<T>(T item) where T : ICacheableDto
        {
            SetAsync<T>(item).Wait();
        }

        public async Task SetAsync<T>(T item, CancellationToken ct = default) where T : ICacheableDto
        {
            string category = GetCategoryOrThrow(typeof(T));
            List<T> cachedItems = await ReadAsync<T>(category, ct);
            var cachedItem = cachedItems.SingleOrDefault(x => x.Id == item.Id);
            if (cachedItem != null)
            {
                cachedItems.Remove(cachedItem);
            }

            cachedItems.Add(item);
            await SerializeToCache<T>(category, cachedItems, ct);
        }

        private string GetCategoryOrThrow(Type itemType)
        {
            return !_domainTypes.ContainsKey(itemType)
                ? throw new Exception("Unsupported type of cacheable object")
                : _domainTypes[itemType];
        }

        private async Task<List<T>> ReadAsync<T>(string category, CancellationToken ct = default)
            where T : ICacheableDto
        {
            byte[] data = await _cache.GetAsync(category, ct);
            if (data == null)
            {
                return new List<T>();
            }

            string jsonContent = Encoding.UTF8.GetString(data);
            List<T> deserializedResult = JsonSerializer.Deserialize<List<T>>(jsonContent);
            return deserializedResult;
        }

        private async Task SerializeToCache<T>(string category, List<T> items, CancellationToken ct = default)
            where T : ICacheableDto
        {
            string jsonContent = JsonSerializer.Serialize(items);
            byte[] data = Encoding.UTF8.GetBytes(jsonContent);
            await _cache.SetAsync(category, data, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            }, ct);
        }
    }
}
