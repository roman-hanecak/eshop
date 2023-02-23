using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public const string GetBasketRouteName = "getbasket";

        [HttpPost("{basket_id}/items")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BasketDto))]
        [SwaggerOperation(
            summary: "Add product to the basket",
            description: "Associate given productId with actual basket of the user, which is represented by basketId",
            OperationId = "AddToBasket",
            Tags = new[] { "Basket Management" })]
        public async Task<IActionResult> AddToBasketAsync(
            [Required, FromRoute(Name = "basket_id")] Guid? basketId,
            [Bind, FromBody] BasketItemModel model,
            CancellationToken ct)
        {
            BasketDto basketDto = await _basketService.AddItemAsync(basketId.Value, model, ct);
            return CreatedAtAction(
                GetBasketRouteName,
                new { basket_id = basketId },
                basketDto);
        }

        [HttpGet("{basket_id}", Name = GetBasketRouteName)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BasketDto))]
        [SwaggerOperation(
            summary: "Retrieve basket",
            description: "Returns basket by given basketId with all products",
            OperationId = "GetBasket",
            Tags = new[] { "Basket API" })]
        public async Task<IActionResult> GetBasketAsync(
            [Required, FromRoute(Name = "basket_id")] Guid? basketId,
            CancellationToken ct)
        {
            BasketDto basketDto = await _basketService.GetAsync(basketId.Value, ct);
            return Ok(basketDto);
        }

        [HttpPut("{basket_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Update basket",
            description: "Update basket based on given basketId with items collection and their quantities",
            OperationId = "UpdateBasket",
            Tags = new[] { "Basket Management" })]
        public async Task<IActionResult> UpdateBasketAsync(
            [Required, FromRoute(Name = "basket_id")] Guid? basketId,
            [FromBody, Bind] BasketModel model,
            CancellationToken ct)
        {
            await _basketService.UpdateAsync(basketId.Value, model, ct);
            return NoContent();
        }

        [HttpDelete("{basket_id}/items/{item_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Remove item from basket",
            description: "Remove item from the basket by given basketId and itemId",
            OperationId = "RemoveItemFromBasket",
            Tags = new[] { "Basket Management" })]
        public async Task<IActionResult> DeleteFromBasketAsync(
            [Required, FromRoute(Name = "basket_id")] Guid? basketId,
            [Required, FromRoute(Name = "item_id")] Guid? itemId,
            CancellationToken ct)
        {
            await _basketService.DeleteItemAsync(basketId.Value, itemId.Value, ct);
            return NoContent();
        }

        [HttpPost("{basket_id}/orders")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerOperation(
            summary: "Create order from basket content",
            description: "Get basket by given basketId and create order",
            OperationId = "CreateOrder",
            Tags = new[] { "Order Management" })]
        public async Task<IActionResult> CreateOrderAsync(
            [Required, FromRoute(Name = "basket_id")] Guid? basketId,
            [FromServices] IOrderService orderService,
            CancellationToken ct)
        {
            BasketDto basket = await _basketService.GetAsync(basketId.Value, ct);
            OrderDto dto = await orderService.CreateAsync(basket, ct);
            return StatusCode(StatusCodes.Status201Created, ct);
        }
    }
}
