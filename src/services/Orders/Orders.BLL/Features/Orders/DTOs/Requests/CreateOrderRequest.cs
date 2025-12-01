using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.ValidationAttributes;
using System.Linq;
using System.Threading.Tasks;
using Orders.BLL.Features.OrderItems.DTOs.Requests;

namespace Orders.BLL.Features.Orders.DTOs.Requests
{
    public record CreateOrderRequest(
        Guid CustomerId,
        DateTime DeliveryDate
    );
}