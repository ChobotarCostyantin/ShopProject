using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Enums;

namespace Orders.BLL.Features.Orders.DTOs.Requests
{
    public record UpdateOrderRequest(
        [Required(ErrorMessage = "Delivery date is required")] DateTime DeliveryDate,
        [Required(ErrorMessage = "Status is required")] Status Status
    );
}