using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Orders.DAL.Database.ConnectionAccessor;
using Orders.DAL.Repositories.Interfaces;
using Orders.DAL.Repositories.UOW.Interfaces;

namespace Orders.DAL.Repositories.UOW.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseConnectionAccessor _databaseConnectionAccessor;
        private DbConnection? _connection;
        private DbTransaction? _transaction;

        public ICustomerRepository CustomerRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IOrderItemRepository OrderItemRepository { get; }
        public IOrderShippingRepository OrderShippingRepository { get; }

        public UnitOfWork(
            IDatabaseConnectionAccessor databaseConnectionAccessor,
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IOrderShippingRepository orderShippingRepository)
        {
            _databaseConnectionAccessor = databaseConnectionAccessor;
            CustomerRepository = customerRepository;
            OrderRepository = orderRepository;
            OrderItemRepository = orderItemRepository;
            OrderShippingRepository = orderShippingRepository;
        }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
        {
            if (_connection != null)
                throw new InvalidOperationException("Transaction already started.");

            _connection = _databaseConnectionAccessor.GetConnection();
            await _connection.OpenAsync();
            _transaction = await _connection.BeginTransactionAsync(isolationLevel);

            (CustomerRepository as RepositoryBase)?.SetTransaction(_connection, _transaction);
            (OrderRepository as RepositoryBase)?.SetTransaction(_connection, _transaction);
            (OrderItemRepository as RepositoryBase)?.SetTransaction(_connection, _transaction);
            (OrderShippingRepository as RepositoryBase)?.SetTransaction(_connection, _transaction);
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("Transaction is not started");
            await _transaction.CommitAsync();
            await _connection!.CloseAsync();
            await CleanupAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("Tranasction is not started");
            await _transaction.RollbackAsync();
            await CleanupAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await CleanupAsync();
        }

        private async Task CleanupAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
                _connection = null;
            }
        }
    }
}