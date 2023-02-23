using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO
{
    public class CatalogDto : CatalogBaseDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ProductDto> Products { get; set; }
    }
}
