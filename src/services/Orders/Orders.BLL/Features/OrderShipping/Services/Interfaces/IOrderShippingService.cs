using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.BLL.Features.OrderShipping.DTOs.Requests;
using Orders.BLL.Features.OrderShipping.DTOs.Responces;
using Shared.ErrorHandling;

namespace Orders.BLL.Features.OrderShipping.Services.Interfaces
{
    public interface IOrderShippingService
    {
        Task<Result<OrderShippingDto?>> GetOrderShippingByIdAsync(Guid shippingId, CancellationToken cancellationToken);
        Task<Result<OrderShippingDto?>> GetOrderShippingByOrderIdAsync(Guid orderId, CancellationToken cancellationToken);
        Task<Result<OrderShippingDto>> CreateOrderShippingAsync(CreateOrderShippingRequest request, CancellationToken cancellationToken);
        Task<Result<OrderShippingDto>> UpdateOrderShippingAsync(Guid shippingId, UpdateOrderShippingRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DeleteOrderShippingAsync(DeleteOrderShippingRequest request, CancellationToken cancellationToken);
    }
}