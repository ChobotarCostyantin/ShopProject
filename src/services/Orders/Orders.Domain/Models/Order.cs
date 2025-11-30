using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Enums;

namespace Orders.Domain.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public required decimal TotalPrice { get; set; }
        public Status Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<OrderItem> OrderItems { get; set; } = [];
    }
}