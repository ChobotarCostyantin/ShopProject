using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.ValidationAttributes;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.Order.DTOs.Requests
{
    public record CreateOrderRequest(
        [Required(ErrorMessage = "Customer id is required")] Guid CustomerId,

        [NotInPast]
        [Required(ErrorMessage = "Delivery date is required")] DateTimeOffset DeliveryDate
    );
}