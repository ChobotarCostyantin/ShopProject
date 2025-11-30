using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.BLL.Features.OrderItems.DTOs.Requests;
using Orders.BLL.Features.OrderItems.DTOs.Responces;
using Orders.BLL.Features.Orders.DTOs.Requests;
using Orders.BLL.Features.Orders.DTOs.Responces;
using Shared.DTOs;
using Shared.ErrorHandling;

namespace Orders.BLL.Features.Orders.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderDto?>> GetOrderByIdAsync(GetOrderByIdRequest request, CancellationToken cancellationToken);
        Task<Result<PaginationResult<OrderDto>>> GetOrdersByCustomerIdAsync(Guid customerId, GetOrdersByCustomerIdRequest request, CancellationToken cancellationToken);
        Task<Result<PaginationResult<OrderDto>>> GetOrdersAsync(GetOrdersRequest request, CancellationToken cancellationToken);
        Task<Result<OrderDto>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
        Task<Result<OrderDto>> UpdateOrderAsync(Guid orderId, UpdateOrderRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteOrderAsync(DeleteOrderRequest request, CancellationToken cancellationToken);
    }
}