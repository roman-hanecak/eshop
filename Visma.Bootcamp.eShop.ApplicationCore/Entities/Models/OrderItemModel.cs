using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.Models
{
    public class OrderItemModel
    {
        [Required]
        public Guid? ProductId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public OrderItem ToDomain()
        {
            return new OrderItem
            {
                Quantity = this.Quantity
            };
        }


    }
}