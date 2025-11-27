using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.Customers.DTOs.Requests
{
    public record DeleteCustomerRequest(
        [Required(ErrorMessage = "Customer Id is required")]
        Guid CustomerId
    );
}