using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BLL.DTOs.Categories.Responces;
using Catalog.BLL.DTOs.ProductDetails.Responces;
using Catalog.BLL.DTOs.Tags.Responces;
using Catalog.DAL.Models;

namespace Catalog.BLL.DTOs.Products.Responces
{
    public record ProductDto(
        Guid ProductId,
        CategoryDto Category,
        string Name,
        string Sku,
        decimal Price,
        int StockQuantity,
        ProductDetailsDto ProductDetails,
        TagDto[] Tags
    );
}