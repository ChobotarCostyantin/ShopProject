using Orders.BLL.Features.Orders.DTOs.Responces;
using Shared.DTOs;

namespace Aggregator.Services;

public class OrdersClient
{
    private readonly HttpClient _client;

    public OrdersClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<PaginationResult<OrderDto>?> GetOrdersByProductIdAsync(Guid productId, CancellationToken ct)
    {
        return await _client.GetFromJsonAsync<PaginationResult<OrderDto>>($"api/orders/by-product/{productId}", ct);
    }
}