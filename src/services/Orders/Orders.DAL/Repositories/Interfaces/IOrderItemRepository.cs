using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Models;

namespace Orders.DAL.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetOrderItemAsync(Guid orderItemId, CancellationToken cancellationToken);
        Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken);
        Task<OrderItem> UpdateOrderItemAsync(Guid orderItemId, OrderItem orderItem, CancellationToken cancellationToken);
        Task<bool> DeleteOrderItemAsync(Guid orderItemId, CancellationToken cancellationToken);
        Task<List<OrderItem>> GetOrderItemsAsync(Guid orderId, int pageSize, int pageNumber, CancellationToken cancellationToken);
        Task<long> CountOrderItemsInOrderAsync(Guid orderId, CancellationToken cancellationToken);
    }
}