using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Database;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private ApplicationContext _context;

        public ProductService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<ProductDto> CreateAsync(
            Guid catalogId,
            ProductModel model,
            CancellationToken ct = default)
        {
            if (await _context.Products.AnyAsync(x => x.Name == model.Name, ct))
            {
                throw new ConflictException($"Product with name {model.Name} already exists");
            }

            var catalog = await _context.Catalogs
                .SingleOrDefaultAsync(x => x.PublicId == catalogId, ct);

            if (catalog == null)
            {
                throw new NotFoundException($"Catalog doesn't exist");
            }

            var product = model.ToDomain();
            product.PublicId = Guid.NewGuid();
            product.CatalogId = catalog.Id;

            await _context.Products.AddAsync(product, ct);
            await _context.SaveChangesAsync(ct);
            return product.ToDto();
        }

        public async Task DeleteAsync(
            Guid productId,
            CancellationToken ct = default)
        {
            var product = await _context.Products
                .SingleOrDefaultAsync(x => x.PublicId == productId, ct);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID: {productId} doesn't exist");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<ProductDto> GetAsync(
            Guid productId,
            CancellationToken ct = default)
        {
            var product = await _context.Products
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.PublicId == productId, ct);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID: {productId} doesn't exist");
            }

            return product.ToDto();
        }

        public async Task<ProductDto> UpdateAsync(
            Guid productId,
            ProductModel model,
            CancellationToken ct = default)
        {
            var product = await _context.Products
                .SingleOrDefaultAsync(x => x.PublicId == productId, ct);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID: {productId} doesn't exist");
            }

            if (await _context.Products.AnyAsync(x => x.Name == model.Name && x.PublicId != productId, ct))
            {
                throw new ConflictException($"Product with name {model.Name} already exists");
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync(ct);
            return product.ToDto();
        }
    }
}
