using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
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
    public class CatalogsController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogsController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public const string GetCatalogRouteName = "getcatalog";

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CatalogBaseDto>))]
        [SwaggerOperation(
            summary: "Retrieve all catalogs from the system",
            description: "Return all catalogs with their products",
            OperationId = "GetCatalogs",
            Tags = new[] { "Catalog API" })]
        public async Task<IActionResult> GetCatalogsAsync(CancellationToken ct)
        {
            List<CatalogBaseDto> listOfCatalogs = await _catalogService.GetAllAsync(ct: ct);
            return Ok(listOfCatalogs);
        }

        [HttpGet("{catalog_id}", Name = GetCatalogRouteName)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CatalogDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Retrieve catalogue based on its Id",
            description: "Return catalog given by catalogId and all its products associated to it",
            OperationId = "GetCatalog",
            Tags = new[] { "Catalog API" })]
        public async Task<IActionResult> GetCatalogAsync(
            [Required, FromRoute(Name = "catalog_id")] Guid? catalogId,
            CancellationToken ct)
        {
            CatalogDto catalogDto = await _catalogService.GetAsync(catalogId.Value, ct);
            return Ok(catalogDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CatalogDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            summary: "Create new catalog",
            description: "Create new catalog in the database",
            OperationId = "CreateCatalog",
            Tags = new[] { "Catalog Management" })]
        public async Task<IActionResult> CreateCatalogAsync(
            [FromBody, Bind] CatalogModel model,
            CancellationToken ct)
        {
            CatalogDto catalogDto = await _catalogService.CreateAsync(model, ct);
            return CreatedAtRoute(
                GetCatalogRouteName,
                new { catalog_id = catalogDto.PublicId },
                catalogDto);
        }

        [HttpPut("{catalog_id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CatalogDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerOperation(
            summary: "Update existing catalog",
            description: "Create new catalog in the database",
            OperationId = "UpdateCatalog",
            Tags = new[] { "Catalog Management" })]
        public async Task<IActionResult> UpdateCatalogAsync(
            [Required, FromRoute(Name = "catalog_id")] Guid? catalogId,
            [FromBody, Bind] CatalogModel model,
            CancellationToken ct)
        {
            CatalogDto catalogDto = await _catalogService.UpdateAsync(catalogId.Value, model, ct);
            return Ok(catalogDto);
        }

        [HttpDelete("{catalog_id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Delete existing catalog",
            description: "Delete catalog from the database with all its products",
            OperationId = "DeleteCatalog",
            Tags = new[] { "Catalog Management" })]
        public async Task<IActionResult> DeleteCatalogAsync(
            [Required, FromRoute(Name = "catalog_id")] Guid? catalogId,
            CancellationToken ct)
        {
            await _catalogService.DeleteAsync(catalogId.Value, ct);
            return NoContent();
        }

        [HttpPost("{catalog_id}/products")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            summary: "Create product in existing catalog",
            description: "Create product in the database and associate it with catalog",
            OperationId = "CreateProductWithCatalogId",
            Tags = new[] { "Product Management" })]
        public async Task<IActionResult> AddProductToCatalogAsync(
            [Required, FromRoute(Name = "catalog_id")] Guid? catalogId,
            [Bind, FromBody] ProductModel model,
            [FromServices] IProductService productService,
            CancellationToken ct)
        {
            ProductDto productDto = await productService.CreateAsync(catalogId.Value, model, ct);
            return CreatedAtRoute(
                ProductsController.GetProductRouteName,
                new { product_id = productDto.PublicId },
                productDto);
        }
    }
}
