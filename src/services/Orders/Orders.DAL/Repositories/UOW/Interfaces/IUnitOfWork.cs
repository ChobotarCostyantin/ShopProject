using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Orders.DAL.Repositories.Interfaces;

namespace Orders.DAL.Repositories.UOW.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ICustomerRepository CustomerRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IOrderShippingRepository OrderShippingRepository { get; }

        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead);
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}