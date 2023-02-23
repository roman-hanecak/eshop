using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO
{
    public class BasketDto : ICacheableDto
    {
        public Guid BasketId { get; set; }
        public IList<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

        [JsonIgnore]
        public Guid Id => BasketId;
    }
}
