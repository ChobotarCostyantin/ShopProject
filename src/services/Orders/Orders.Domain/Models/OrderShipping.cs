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
        public required string AdressLine { get; set; }
        public required string City { get; set; }
        public string? PostalCode { get; set; }
    }
}