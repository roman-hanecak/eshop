using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public decimal Price { get; set; }


        public OrderItemDto ToDto()
        {
            return new OrderItemDto
            {
                Quantity = this.Quantity,
                Price = this.Price,
                ProductId = this.Product.PublicId
            };
        }
    }
}