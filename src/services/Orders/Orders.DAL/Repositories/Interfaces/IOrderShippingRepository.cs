using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Models;

namespace Orders.DAL.Repositories.Interfaces
{
    public interface IOrderShippingRepository
    {
        Task<OrderShipping?> GetOrderShippingAsync(Guid orderId, CancellationToken cancellationToken);
        Task<OrderShipping> CreateOrderShippingAsync(OrderShipping orderShipping, CancellationToken cancellationToken);
        Task<OrderShipping> UpdateOrderShippingAsync(Guid orderShippingId, OrderShipping orderShipping, CancellationToken cancellationToken);
        Task<bool> DeleteOrderShippingAsync(OrderShipping orderShipping, CancellationToken cancellationToken);
        Task<OrderShipping?> GetOrderShippingByOrderIdAsync(Guid orderId, CancellationToken cancellationToken);
    }
}