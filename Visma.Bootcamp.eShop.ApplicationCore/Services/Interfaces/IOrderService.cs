using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces
{
    public interface IOrderService
    {
        #region Order Operations
        Task<OrderDto> GetAsync(Guid? orderId, CancellationToken ct = default);
        Task<List<OrderDto>> GetAllAsync(int pageSize = 10, CancellationToken ct = default);
        Task<List<OrderDto>> GetAllForUserAsync(Guid? userId, CancellationToken ct = default);
        Task<OrderDto> CreateAsync(BasketDto basket, CancellationToken ct = default);
        Task DeleteAsync(Guid? orderId, CancellationToken ct = default);
        Task<OrderDto> UpdateAsync(Guid? orderId, OrderModel model, CancellationToken ct = default);
        #endregion


        #region Order Item Operations
        Task<OrderItemDto> AddItemAsync(Guid orderId, OrderItemModel model, CancellationToken ct = default);
        Task DeleteItemAsync(Guid orderId, Guid? productId, CancellationToken ct = default);
        #endregion

        #region Order Cancelation Operations
        Task CancelOrderAsync(Guid? orderId, CancellationToken ct = default);


        #endregion

    }
}
