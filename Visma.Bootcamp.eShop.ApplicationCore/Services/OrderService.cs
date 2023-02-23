using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Visma.Bootcamp.eShop.ApplicationCore.Database;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Models;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationContext _context;
        public async Task<OrderItemDto> AddItemAsync(Guid orderId, OrderItemModel model, CancellationToken ct = default)
        {
            Order order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync(x => x.PublicId == orderId, ct);

            if (order == null)
            {
                throw new NotFoundException($"Order with ID: {orderId} not found");
            }

            Product product = await _context.Products
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.PublicId == model.ProductId, ct);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID: {model.ProductId} not found");
            }

            OrderItem item = order.Items.SingleOrDefault(x => x.ProductId == product.Id);
            if (item == null)
            {
                item = model.ToDomain();
                item.ProductId = product.Id;
                item.Price = product.Price;

                order.Items.Add(item);
                _context.Orders.Update(order);
            }
            else
            {
                item.Quantity += model.Quantity;
                _context.OrderItems.Update(item);
            }

            await _context.SaveChangesAsync(ct);
            OrderItemDto dto = item.ToDto();
            return dto;
        }

        public async Task CancelOrderAsync(Guid? orderId, CancellationToken ct = default)
        {
            var order = await _context.Orders.AsNoTracking().SingleOrDefaultAsync(o => o.PublicId == orderId);
            //throw new NotImplementedException();
            if (order == null)
            {
                throw new NotFoundException($"Order with {orderId} doesnt exists");
            }

            order.Status = (int)OrderStatus.Cancelled;
            //var orderDto = order.ToDto();
            await _context.SaveChangesAsync(ct);
            //return orderDto;
        }

        public async Task<OrderDto> CreateAsync(BasketDto basket, CancellationToken ct = default)
        {
            var order = new Order
            {
                CreatedDate = DateTime.UtcNow,
                PublicId = Guid.NewGuid(),
                Items = new List<OrderItem>()
            };

            foreach (BasketItemDto basketItem in basket.Items)
            {
                Product product = await _context.Products
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.PublicId == basketItem.Product.PublicId, ct);

                if (product == null)
                {
                    throw new NotFoundException($"Product with ID: {basketItem.Product.PublicId} not found");
                }

                var orderItem = new OrderItem
                {
                    Quantity = basketItem.Quantity,
                    Price = basketItem.Product.Price,
                    ProductId = product.Id
                };

                order.Items.Add(orderItem);
            }

            await _context.Orders.AddAsync(order, ct);
            await _context.SaveChangesAsync(ct);

            await _context.Entry(order)
                .Collection(x => x.Items)
                .Query()
                .Include(x => x.Product)
                .LoadAsync();

            OrderDto dto = order.ToDto(includeItems: true);
            return dto;
        }

        public async Task DeleteAsync(Guid? orderId, CancellationToken ct = default)
        {
            var order = await _context.Orders.AsNoTracking().Include(x => x.PublicId == orderId).SingleOrDefaultAsync();
            if (order == null)
            {
                throw new Exception("Order doesnt exists");
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(Guid orderId, Guid? productId, CancellationToken ct = default)
        {
            Order order = await _context.Orders
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync(x => x.PublicId == orderId, ct);

            if (order == null)
            {
                throw new NotFoundException($"Order with ID: {orderId} not found");
            }

            OrderItem item = order.Items.SingleOrDefault(x => x.Product.PublicId == productId);
            if (item == null)
            {
                throw new NotFoundException($"OrderItem with ID: {productId} not found");
            }

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<List<OrderDto>> GetAllAsync(int pageSize = 10, CancellationToken ct = default)
        {
            throw new NotImplementedException();

            var query = _context.Orders.AsNoTracking(); //.AsQueryable();

            List<Order> orders = await query.ToListAsync(ct);

            List<OrderDto> orderDtos = orders.Select(x => x.ToDto()).ToList();

            return orderDtos;
        }

        public Task<List<OrderDto>> GetAllForUserAsync(Guid? userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDto> GetAsync(Guid? orderId, CancellationToken ct = default)
        {
            var order = await _context.Orders.AsNoTracking().Include(x => x.Items).SingleOrDefaultAsync(o => o.PublicId == orderId);
            //throw new NotImplementedException();

            var orderDto = order.ToDto();

            return orderDto;
        }

        public async Task<OrderDto> UpdateAsync(Guid? orderId, OrderModel model, CancellationToken ct = default)
        {
            Order order = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync(x => x.PublicId == orderId);

            if (order == null)
            {
                throw new NotFoundException($"Order with ID: {orderId} not found");
            }

            foreach (OrderItemModel item in model.Items)
            {
                OrderItem orderItem = order.Items
                    .SingleOrDefault(x => x.Product.PublicId == item.ProductId);

                if (orderItem == null)
                {
                    continue;
                }

                orderItem.Quantity = item.Quantity;
                _context.OrderItems.Update(orderItem);
            }

            await _context.SaveChangesAsync(ct);
            OrderDto dto = order.ToDto();
            return dto;

        }
    }
}
