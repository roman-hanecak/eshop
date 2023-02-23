using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO
{
    public class OrderItemDto
    {
        public Guid? ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}