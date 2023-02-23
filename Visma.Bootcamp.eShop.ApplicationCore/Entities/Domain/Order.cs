using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.Domain
{
    // db entity
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid? PublicId { get; set; }

        public int Status { get; set; } = (int)OrderStatus.Created;

        [Required]
        public DateTime? CreatedDate { set; get; }

        public string UserId { get; set; } = "Roman";
        public virtual ICollection<OrderItem> Items { get; set; }

        public OrderDto ToDto(bool includeItems = false)
        {
            return new OrderDto
            {
                PublicId = this.PublicId,
                CreatedDate = this.CreatedDate,
                Items = includeItems ? this.Items.Select(i => i.ToDto()).ToList() : null,
                Amount = includeItems ? CalculateAmount() : 0,
                Status = this.Status
            };
        }

        private decimal CalculateAmount()
        {
            if (this.Items == null || this.Items.Count == 0)
            {
                return 0;
            }
            return this.Items.Sum(i => i.Price * i.Quantity);
        }

    }
}
