using System;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketDto> GetAsync(Guid basketId, CancellationToken ct = default);

        Task UpdateAsync(Guid basketId, BasketModel model, CancellationToken ct = default);

        Task<BasketDto> AddItemAsync(Guid basketId, BasketItemModel model, CancellationToken ct = default);
        Task DeleteItemAsync(Guid basketId, Guid itemId, CancellationToken ct = default);
    }
}
