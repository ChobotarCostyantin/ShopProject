using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.BLL.Features.OrderShipping.DTOs.Responces
{
    public record OrderShippingDto(
        Guid OrderId,
        Guid ShippingId,
        string AddressLine,
        string City,
        string PostalCode
    );
}