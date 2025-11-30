using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.OrderShipping.DTOs.Requests
{
    public record UpdateOrderShippingRequest(
        [Required(ErrorMessage = "Address line is required")] [MaxLength(100, ErrorMessage = "Address line cannot be longer than 100 characters")] string AddressLine,
        [Required(ErrorMessage = "City is required")] [MaxLength(50, ErrorMessage = "City cannot be longer than 50 characters")] string City,
        [Required(ErrorMessage = "Posrtal code is required")] [MaxLength(10, ErrorMessage = "Postal code cannot be longer than 10 characters")] string PostalCode
    );
}