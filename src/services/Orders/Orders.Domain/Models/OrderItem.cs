using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Domain.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public required decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}