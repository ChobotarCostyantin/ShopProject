using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Enums;
using Shared.ValidationAttributes;

namespace Orders.BLL.Features.Orders.DTOs.Requests
{
    public record UpdateOrderRequest(
        DateTime DeliveryDate,
        Status Status
    );
}