using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services
{
    public class BasketService : IBasketService
    {
        private readonly ICacheManager _cache;
        private readonly IProductService _productService;

        public BasketService(ICacheManager cache, IProductService productService)
        {
            _cache = cache;
            _productService = productService;
        }

        public async Task<BasketDto> AddItemAsync(
            Guid basketId,
            BasketItemModel model,
            CancellationToken ct = default)
        {
            /*
            1. checknem ci basket existuje
            - ak nie, vytvorim ho
            2*- skontrolovat ci product existuje (out of scope)
            2. skontrolujem ci productId uz existuje v kosiku
            - ak nie, tak ho vytvorim
            - ak ano, zvysim quantity
            3. ulozim basket spat do pamate
            Errory:
            - ak je quantita <= 0 > 20 vratim 
            - 400 bad request, 409 conflict, 422 unprocessable entity
            */
            const int maxItems = 20;
            if (model.Quantity <= 0 || model.Quantity > maxItems)
            {
                throw new BadRequestException("Quantity must be a number between 1 and 20");
            }

            ProductDto product = await _productService.GetAsync(model.ProductId.Value, ct);
            var (_, basket) = await GetBasketAsync(basketId, true, ct);

            var basketItem = basket.Items
                .SingleOrDefault(x => x.Product.PublicId == model.ProductId);
            if (basketItem != null)
            {
                basketItem.Quantity += model.Quantity;
                if (basketItem.Quantity > maxItems)
                {
                    throw new UnprocessableEntityException("You can order maximum of 20 items in a single request");
                }
            }
            else
            {
                basket.Items.Add(new BasketItemDto
                {
                    Product = new ProductDto
                    {
                        PublicId = product.PublicId,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price
                    },
                    Quantity = model.Quantity,
                });
            }

            // save back to memory
            await _cache.SetAsync(basket, ct);

            return basket;
        }

        public async Task DeleteItemAsync(
            Guid basketId,
            Guid itemId,
            CancellationToken ct = default)
        {
            // co ak basket neexistuje
            // - hodim not found
            // ak item existuje
            // - zmazem ho
            // ak neexsituje 
            // - hodim not found
            // ak vsetko prebehlo OK, ulozim spat do cache

            var (basketExists, basket) = await GetBasketAsync(basketId, ct: ct);
            if (!basketExists)
            {
                throw new NotFoundException($"Basket with ID: {basketId} not found");
            }

            var basketItem = basket.Items
                .SingleOrDefault(x => x.Product.PublicId == itemId);
            if (basketItem == null)
            {
                throw new NotFoundException($"Product with ID: {itemId} is not present in the basket");
            }

            basket.Items.Remove(basketItem);
            await _cache.SetAsync(basket, ct);
        }

        public async Task<BasketDto> GetAsync(
            Guid basketId,
            CancellationToken ct = default)
        {
            var (_, basket) = await GetBasketAsync(basketId, true, ct);
            return basket;
        }

        public async Task UpdateAsync(
            Guid basketId,
            BasketModel model,
            CancellationToken ct = default)
        {
            // 1. ak basket neexistuje co s tym?
            // 2. preforeachujem itemy a zistim
            // - ci item existuje?
            //   - ak ano updatnem quantity
            //   - ak nie, co potom?
            // 3. ulozim kosik

            // check if item quantities are within range
            if (model.Items.Any(x => x.Quantity <= 0 || x.Quantity > 20))
            {
                throw new BadRequestException("Quantity must be a number between 1 and 20");
            }

            // get basket and check if it exists
            var (basketExists, basket) = await GetBasketAsync(basketId, ct: ct);
            if (!basketExists)
            {
                throw new NotFoundException($"Basket with ID: {basketId} not found");
            }

            foreach (BasketItemModel item in model.Items)
            {
                var basketItem = basket.Items
                    .SingleOrDefault(x => x.Product.PublicId == item.ProductId);
                if (basketItem == null)
                {
                    throw new NotFoundException($"Product with ID: {item.ProductId} not found in the basket");
                }

                basketItem.Quantity = item.Quantity;
            }

            await _cache.SetAsync(basket, ct);
        }

        private async Task<(bool, BasketDto)> GetBasketAsync(
            Guid basketId,
            bool createBasket = false,
            CancellationToken ct = default)
        {
            BasketDto basket = await _cache.GetAsync<BasketDto>(basketId, ct);
            if (basket == null)
            {
                if (!createBasket) return (false, null);
                else
                {
                    basket = new BasketDto { BasketId = basketId };
                    await _cache.SetAsync(basket, ct);
                }
            }

            return (true, basket);
        }
    }
}
