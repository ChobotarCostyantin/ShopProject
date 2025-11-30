using System;
using System.Threading;
using System.Threading.Tasks;
using Orders.BLL.Features.OrderItems.DTOs.Requests;
using Orders.BLL.Features.OrderItems.DTOs.Responces;
using Shared.DTOs;
using Shared.ErrorHandling;

namespace Orders.BLL.Features.OrderItems.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<Result<OrderItemDto>> GetOrderItemByIdAsync(GetOrderItemByIdRequest request, CancellationToken cancellationToken);
        Task<Result<PaginationResult<OrderItemDto>>> GetOrderItemsByOrderIdAsync(Guid orderId, GetOrderItemsByOrderIdRequest request, CancellationToken cancellationToken);
        Task<Result<OrderItemDto>> CreateOrderItemAsync(CreateOrderItemRequest request, CancellationToken cancellationToken);
        Task<Result<OrderItemDto>> UpdateOrderItemAsync(Guid orderItemId, UpdateOrderItemRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteOrderItemAsync(DeleteOrderItemRequest request, CancellationToken cancellationToken);
    }
}