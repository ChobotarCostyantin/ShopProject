using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Orders.BLL.Features.Customers.DTOs.Requests
{
    public record CreateCustomerRequest(
        Guid UserId,
        string FullName,
        string Email
    );
}