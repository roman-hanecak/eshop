using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.eShop.ApplicationCore.Infrastructure
{
    public interface ICacheManager
    {
        void Set<T>(T item) where T : ICacheableDto;
        Task SetAsync<T>(T item, CancellationToken ct = default) where T : ICacheableDto;

        List<T> Get<T>() where T : ICacheableDto;
        Task<List<T>> GetAsync<T>(CancellationToken ct = default) where T : ICacheableDto;

        T Get<T>(Guid itemId) where T : ICacheableDto;
        Task<T> GetAsync<T>(Guid itemId, CancellationToken ct = default) where T : ICacheableDto;

        void Remove<T>(Guid itemId) where T : ICacheableDto;
        Task RemoveAsync<T>(Guid itemId, CancellationToken ct = default) where T : ICacheableDto;
    }
}
