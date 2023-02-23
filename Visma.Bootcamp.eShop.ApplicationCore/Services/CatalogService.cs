using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Database;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ApplicationContext _context;

        public CatalogService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<CatalogDto> CreateAsync(
            CatalogModel model,
            CancellationToken ct = default)
        {
            Catalog duplicate = await _context.Catalogs
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Name == model.Name, ct);

            // checking for duplicate
            if (duplicate != null)
            {
                throw new ConflictException($"Catalog with name {model.Name} already exists");
            }

            // convert CatalogModel to Catalog
            Catalog catalog = model.ToDomain();
            catalog.PublicId = Guid.NewGuid();

            // save to the database
            await _context.AddAsync(catalog, ct);
            await _context.SaveChangesAsync(ct);

            // last step
            var catalogDto = catalog.ToDto();
            return catalogDto;
        }

        public async Task DeleteAsync(
            Guid catalogId,
            CancellationToken ct = default)
        {
            var catalog = await _context.Catalogs
                .SingleOrDefaultAsync(x => x.PublicId == catalogId, ct);
            if (catalog == null)
            {
                throw new NotFoundException($"Catalog with ID: {catalogId} doesn't exist");
            }

            _context.Catalogs.Remove(catalog);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<List<CatalogBaseDto>> GetAllAsync(
            Expression<Func<Catalog, bool>> predicate = null,
            CancellationToken ct = default)
        {
            IQueryable<Catalog> query = _context.Catalogs
                .AsNoTracking();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var catalogs = await query.ToListAsync(ct);
            return catalogs.Select(x => (CatalogBaseDto)x.ToDto()).ToList();
        }

        public async Task<CatalogDto> GetAsync(
            Guid catalogId,
            CancellationToken ct = default)
        {
            var catalog = await _context.Catalogs
                .AsNoTracking()
                .Include(x => x.Products)
                .SingleOrDefaultAsync(x => x.PublicId == catalogId, ct);

            if (catalog == null)
            {
                throw new NotFoundException($"Catalog with ID: {catalogId} doesn't exist");
            }

            var catalogDto = catalog.ToDto();
            return catalogDto;
        }

        public async Task<CatalogDto> UpdateAsync(
            Guid catalogId,
            CatalogModel model,
            CancellationToken ct = default)
        {
            if (_context.Catalogs.Any(x => x.Name == model.Name && x.PublicId != catalogId))
            {
                throw new ConflictException($"Catalog with name {model.Name} already exists");
            }

            var catalog = await _context.Catalogs
                .SingleOrDefaultAsync(x => x.PublicId == catalogId, ct);
            if (catalog == null)
            {
                throw new NotFoundException($"Catalog with ID: {catalogId} doesn't exist");
            }

            catalog.Name = model.Name;
            catalog.Description = model.Description;

            _context.Catalogs.Update(catalog);
            await _context.SaveChangesAsync(ct);

            return catalog.ToDto();
        }
    }
}
