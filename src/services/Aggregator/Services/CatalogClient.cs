using Catalog.BLL.DTOs.Products.Responces;
using Shared.DTOs;

namespace Aggregator.Services;

public class CatalogClient
{
    private readonly HttpClient _client;

    public CatalogClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _client.GetFromJsonAsync<ProductDto>($"api/products/{id}", ct);
    }
}