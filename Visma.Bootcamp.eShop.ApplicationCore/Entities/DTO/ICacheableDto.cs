using System;
using System.Text.Json.Serialization;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO
{
    public interface ICacheableDto
    {
        [JsonIgnore]
        Guid Id { get; }
    }
}
