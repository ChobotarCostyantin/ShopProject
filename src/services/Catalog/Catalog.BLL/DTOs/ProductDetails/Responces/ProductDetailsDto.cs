using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BLL.DTOs.ProductDetails.Responces
{
    public record ProductDetailsDto(
        Guid ProductId,
        string? Description,
        string? Manufacturer,
        float Weight_Kg
    );
}