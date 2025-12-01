using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Domain.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public Guid UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
    }
}