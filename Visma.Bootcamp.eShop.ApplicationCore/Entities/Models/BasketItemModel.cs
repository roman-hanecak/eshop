using System;
using System.ComponentModel.DataAnnotations;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.Models
{
    public class BasketItemModel
    {
        [Required]
        public Guid? ProductId { get; set; }

        [Required, Range(1, 20)]
        public int Quantity { get; set; }

        public BasketItemDto ToDto()
        {
            return new BasketItemDto
            {
                Product = new ProductDto
                {
                    PublicId = ProductId.Value
                },
                Quantity = Quantity
            };
        }
    }
}
