using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BLL.DTOs.Products.Requests
{
    public record UpdateProductRequest(
        Guid CategoryId,
        string Name,
        decimal Price,
        int StockQuantity
    );
}