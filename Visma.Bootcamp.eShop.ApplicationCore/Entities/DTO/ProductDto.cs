using System;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO
{
    public class ProductDto
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
