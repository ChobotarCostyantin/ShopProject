using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.OrderItems.DTOs.Requests
{
    public record CreateOrderItemRequest(
        [Required(ErrorMessage = "Order id is required")] Guid OrderId,
        [Required(ErrorMessage = "Product id is required")] Guid ProductId,
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")] int Quantity
    );
}