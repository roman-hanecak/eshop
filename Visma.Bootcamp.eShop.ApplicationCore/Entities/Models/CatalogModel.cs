using System.ComponentModel.DataAnnotations;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.Models
{
    public class CatalogModel
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public Catalog ToDomain()
        {
            return new Catalog
            {
                Name = Name,
                Description = Description
            };
        }
    }
}
