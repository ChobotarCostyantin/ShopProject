using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Domain.Models
{
    public class OrderShipping
    {
        public Guid ShippingId { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public required string AdressLine { get; set; }
        public required string City { get; set; }
        public required string PostalCode { get; set; }
    }
}