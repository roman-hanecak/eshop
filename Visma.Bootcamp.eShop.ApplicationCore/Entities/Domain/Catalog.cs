using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain
{
    // db entity
    public class Catalog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid? PublicId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        // one-to-many
        public virtual ICollection<Product> Products { get; set; }

        public CatalogDto ToDto()
        {
            return new CatalogDto
            {
                Description = Description,
                Name = Name,
                PublicId = PublicId.Value,
                Products = Products?.Select(x => x.ToDto()).ToList()
            };
        }
    }
}
