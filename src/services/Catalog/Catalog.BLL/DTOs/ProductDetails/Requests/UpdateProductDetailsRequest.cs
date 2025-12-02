using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BLL.DTOs.ProductDetails.Requests
{
    public record UpdateProductDetailsRequest(
        string? Description,
        string? Manufacturer,
        float Weight_Kg
    );
}