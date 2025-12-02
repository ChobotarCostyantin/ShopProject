

namespace Catalog.BLL.DTOs.Products.Requests
{
    public record CreateProductRequest(
        Guid CategoryId,
        string Name,
        string Sku,
        decimal Price,
        int StockQuantity
    );
}