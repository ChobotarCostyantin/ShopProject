using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Orders.BLL.Features.Customers.DTOs.Requests
{
    public record CreateCustomerRequest(
        [Required(ErrorMessage = "Full name is required")]
        [MinLength(3, ErrorMessage = "Full name must be at least 3 characters long")]
        [MaxLength(50, ErrorMessage = "Full name must be at most 50 characters long")]
        string FullName,

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        string Email
    );
}