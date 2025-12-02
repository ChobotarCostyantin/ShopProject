using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Common;

namespace Catalog.BLL.DTOs.Products.Requests
{
    public record GetProductsRequest(
        Guid? CategoryId,
        string? SearchTerm,
        decimal? MinPrice,
        decimal? MaxPrice,
        bool? InStock
    ) : GetRequest;
}