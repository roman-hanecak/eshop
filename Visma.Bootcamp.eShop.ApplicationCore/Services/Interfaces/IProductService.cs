using System;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetAsync(Guid productId, CancellationToken ct = default);

        Task<ProductDto> CreateAsync(Guid catalogId, ProductModel model, CancellationToken ct = default);
        Task<ProductDto> UpdateAsync(Guid productId, ProductModel model, CancellationToken ct = default);
        Task DeleteAsync(Guid productId, CancellationToken ct = default);
    }
}
