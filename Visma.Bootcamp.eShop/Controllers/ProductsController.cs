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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public const string GetProductRouteName = "getproduct";

        [HttpGet("{product_id}", Name = GetProductRouteName)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Create product in existing catalog",
            description: "Create product in the database and associate it with catalog",
            OperationId = "CreateProduct",
            Tags = new[] { "Product API" })]
        public async Task<IActionResult> GetProductAsync(
            [Required, FromRoute(Name = "product_id")] Guid? productId,
            CancellationToken ct)
        {
            ProductDto productDto = await _productService.GetAsync(productId.Value, ct);
            return Ok(productDto);
        }

        [HttpPut("{product_id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            summary: "Update product",
            description: "Update product in the database",
            OperationId = "UpdateProduct",
            Tags = new[] { "Product Management" })]
        public async Task<IActionResult> UpdateProductAsync(
            [Required, FromRoute(Name = "product_id")] Guid? productId,
            [Bind, FromBody] ProductModel model,
            CancellationToken ct)
        {
            ProductDto productDto = await _productService.UpdateAsync(productId.Value, model, ct);
            return Ok(productDto);
        }

        [HttpDelete("{product_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Delete product",
            description: "Delete product from the database",
            OperationId = "DeleteProduct",
            Tags = new[] { "Product Management" })]
        public async Task<IActionResult> DeleteProductAsync(
            [Required, FromRoute(Name = "product_id")] Guid? productId,
            CancellationToken ct)
        {
            await _productService.DeleteAsync(productId.Value, ct);
            return NoContent();
        }
    }
}
