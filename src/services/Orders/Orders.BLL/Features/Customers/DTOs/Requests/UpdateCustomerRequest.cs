using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.Customers.DTOs.Requests
{
    public record UpdateCustomerRequest(
        [Required(ErrorMessage = "Full name is required")]
        [MinLength(3, ErrorMessage = "Full name must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Full name must be at most 50 characters")]
        string FullName
    );
}