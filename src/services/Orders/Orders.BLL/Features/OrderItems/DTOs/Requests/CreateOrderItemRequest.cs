using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.OrderItems.DTOs.Requests
{
    public record CreateOrderItemRequest(
        Guid OrderId,
        Guid ProductId,
        int Quantity
    );
}