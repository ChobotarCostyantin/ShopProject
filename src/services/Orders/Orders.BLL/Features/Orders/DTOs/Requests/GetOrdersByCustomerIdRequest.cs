using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTOs;

namespace Orders.BLL.Features.Orders.DTOs.Requests
{
    public record GetOrdersByCustomerIdRequest : PaginationRequest;
}