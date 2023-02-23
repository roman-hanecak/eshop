using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Visma.Bootcamp.eShop.ApplicationCore.Entities.Models
{
    public class OrderModel
    {
        public List<OrderItemModel> Items { get; set; }
    }
}