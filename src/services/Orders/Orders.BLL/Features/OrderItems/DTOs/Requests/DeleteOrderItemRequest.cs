using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.OrderItems.DTOs.Requests
{
    public record DeleteOrderItemRequest(
        [Required(ErrorMessage = "OrderItemId is required")] Guid OrderItemId
    );
}