using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Domain.Enums;
using Orders.Domain.Models;

namespace Orders.DAL.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken);
        Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken);
        Task<Order> UpdateOrderAsync(Guid orderId, Order order, CancellationToken cancellationToken);
        Task<bool> DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken);
        Task<long> CountAllOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
        Task<long> CountAllOrdersAsync(CancellationToken cancellationToken);
        Task<List<Order>> GetOrdersByCustomerIdAsync(Guid customerId, int pageSize, int pageNumber, CancellationToken cancellationToken);
        Task<List<Order>> GetOrdersAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
    }
}