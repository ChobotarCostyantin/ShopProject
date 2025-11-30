using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.OrderItems.DTOs.Responces
{
    public record OrderItemDto(
        Guid OrderItemId,
        Guid OrderId,
        Guid ProductId,
        int Quantity
    );
}