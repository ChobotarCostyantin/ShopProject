using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.BLL.Features.OrderItems.DTOs.Responces;
using Orders.Domain.Enums;

namespace Orders.BLL.Features.Orders.DTOs.Responces
{
    public record OrderDto(
        Guid OrderId,
        Guid CustomerId,
        DateTimeOffset DeliveryDate,
        decimal TotalPrice,
        Status Status,
        DateTimeOffset CreatedAt,
        OrderItemDto[] OrderItems
    );
}