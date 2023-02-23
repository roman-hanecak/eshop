using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/orders-cancellation")]
    public class OrdersCancellationController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersCancellationController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("{order_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Cancel order",
            description: "Cancel order by given orderId and set its status to Cancelled",
            OperationId = "CancelOrder",
            Tags = new[] { "Order Management" })]
        public async Task<IActionResult> CancelOrderAsync(
            [Required, FromRoute(Name = "order_id")] Guid? orderId,
            CancellationToken ct)
        {
            await _orderService.CancelOrderAsync(orderId);
            return NoContent();
        }
    }
}
