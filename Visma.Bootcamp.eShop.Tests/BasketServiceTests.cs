using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.Tests
{
    [TestFixture]
    public class BasketServiceTests
    {
        private ICacheManager _cacheManager;
        private IProductService _productService;
        private IBasketService _basketService;

        [SetUp]
        public void setUp()
        {
            _cacheManager = Substitute.For<ICacheManager>();
            _productService = Substitute.For<IProductService>();
            _basketService = Substitute.For<IBasketService>();

        }

        [Test]
        public void AddItemAsync_InvalidQuantity_ThrowsBadRequestException()
        {
            // Arrange
            var basketId = Guid.NewGuid();
            var model = new BasketItemModel { ProductId = Guid.NewGuid(), Quantity = 21 };
            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _basketService.AddItemAsync(basketId, model));
        }

        [Test]
        public async Task AddItemAsync_QuantityExceedsLimit_ThrowsUnprocessableEntityException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var basketId = Guid.NewGuid();
            var model = new BasketItemModel { ProductId = productId, Quantity = 5 };

            var mockedInvalidProduct = new ProductDto
            {
                PublicId = productId,
            };

            var mockedBasket = new BasketDto()
            {
                Items = new List<BasketItemDto>()
                {
                    new BasketItemDto
                    {
                        Product = mockedInvalidProduct,
                        Quantity = 19
                    }
                }
            };

            _cacheManager
                .GetAsync<BasketDto>(basketId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(mockedBasket));
            // Act & Assert
            Assert.ThrowsAsync<UnprocessableEntityException>(() => _basketService.AddItemAsync(basketId, model));
        }

        [Test]
        public async Task DeleteItemAsync_BasketNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            _cacheManager
                .GetAsync<BasketDto>(basketId, Arg.Any<CancellationToken>())
                .ReturnsNull();
            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _basketService.DeleteItemAsync(basketId, productId));
        }

        [Test]
        public async Task DeleteItemAsync_ItemNotPresentInBasket_ThrowsNotFoundException()
        {
            // Arrange
            var basketId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var mockedBasket = new BasketDto()
            {
                Items = new List<BasketItemDto>()
            };
            _cacheManager
                .GetAsync<BasketDto>(basketId, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(mockedBasket));
            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _basketService.DeleteItemAsync(basketId, productId));
        }
    }
}