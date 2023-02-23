using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public const string GetOrderRouteName = "getOrder";

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDto>))]
        [SwaggerOperation(
            summary: "Retrieve all orders from the system",
            description: "Return all orders with their products",
            OperationId = "GetOrders",
            Tags = new[] { "Order API" })]
        public async Task<IActionResult> GetOrdersAsync(CancellationToken ct)
        {
            List<OrderDto> listOfOrders = await _orderService.GetAllAsync(ct: ct);
            return Ok(listOfOrders);
        }

        [HttpGet("{order_id}", Name = GetOrderRouteName)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Retrieve order based on its Id",
            description: "Return order given by catalogId and all its products associated to it",
            OperationId = "GetOrder",
            Tags = new[] { "Order API" })]
        public async Task<IActionResult> GetOrderAsync(
            [Required, FromRoute(Name = "order_id")] Guid? orderId,
            CancellationToken ct)
        {
            OrderDto orderDto = await _orderService.GetAsync(orderId.Value, ct);
            return Ok(orderDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            summary: "Create new order",
            description: "Create new order in the database",
            OperationId = "CreateOrder",
            Tags = new[] { "Order Management" })]
        public async Task<IActionResult> CreateOrderAsync(
            [FromBody, Bind] BasketDto model,
            CancellationToken ct)
        {
            OrderDto orderDto = await _orderService.CreateAsync(model, ct);

            //return StatusCode(StatusCode.Status201Created, orderDto.PublicId);
            //return Ok(orderDto);
            return CreatedAtRoute(
                GetOrderRouteName,
                new { order_id = orderDto.PublicId },
                orderDto);
        }

        [HttpPut("{order_id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            summary: "Update existing order",
            description: "Create new order in the database",
            OperationId = "UpdateOrder",
            Tags = new[] { "Order Management" })]
        public async Task<IActionResult> UpdateOrderAsync(
            [Required, FromRoute(Name = "order_id")] Guid? orderId,
            [FromBody, Bind] OrderModel model,
            CancellationToken ct)
        {
            OrderDto orderDto = await _orderService.UpdateAsync(orderId, model, ct);
            return Ok(orderDto);
        }

        [HttpDelete("{order_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Delete existing order",
            description: "Delete order from the database with all its products",
            OperationId = "DeleteOrder",
            Tags = new[] { "Order Management" })]
        public async Task<IActionResult> DeleteOrderAsync(
            [Required, FromRoute(Name = "order_id")] Guid? orderId,
            CancellationToken ct)
        {
            await _orderService.DeleteAsync(orderId, ct);
            return NoContent();
        }

        // [HttpPost("{order_id}/products")]
        // [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [SwaggerOperation(
        //     summary: "Add product in existing order",
        //     description: "Add product in the database and associate it with order",
        //     OperationId = "AddProductWithOrderId",
        //     Tags = new[] { "Order product Management" })]
        // public async Task<OrderItemDto> AddItemAsync(Guid orderId, OrderItemModel model, CancellationToken ct = default){

        // }

        // public async Task DeleteItemAsync(Guid orderId, Guid? productId, CancellationToken ct = default)



        /*
            Pre kazdu operaciu v tomto controlleri, nezabudnite pridat
            [SwaggerOperation(
                summary: "Kratky popis endpointu",
                description: "Trosku dlhsi popis endpointu, kludne aj detaily business logiky",
                OperationId = "JedinecneIdOperacie",
                Tags = new[] { "Order API" })]
        */

        // Navrh metod do tohto controllera:
        // - GET {order_id}
        // - DELETE {order_id]
        // - PUT {order_id}
    }
}
