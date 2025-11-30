using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.OrderShipping.DTOs.Requests
{
    public record DeleteOrderShippingRequest(
        [Required(ErrorMessage = "ShippingId is required")] Guid ShippingId
    );
}