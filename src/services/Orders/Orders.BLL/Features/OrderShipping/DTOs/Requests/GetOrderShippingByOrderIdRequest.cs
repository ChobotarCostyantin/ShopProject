using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.OrderShipping.DTOs.Requests
{
    public record GetOrderShippingByOrderIdRequest(Guid OrderId);
}