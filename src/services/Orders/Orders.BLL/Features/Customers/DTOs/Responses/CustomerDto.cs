using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.Customers.DTOs.Responses
{
    public record CustomerDto(
        Guid CustomerId,
        Guid UserId,
        string FullName,
        string Email
    );
}