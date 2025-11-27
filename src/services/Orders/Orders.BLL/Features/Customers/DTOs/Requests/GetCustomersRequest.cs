using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTOs;

namespace Orders.BLL.Features.Customers.DTOs.Requests
{
    public record GetCustomersRequest : PaginationRequest;
}