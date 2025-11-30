using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.Orders.DTOs.Requests
{
    public record GetOrderByIdRequest(
        [Required(ErrorMessage = "Order id is required")] Guid OrderId
    );
}