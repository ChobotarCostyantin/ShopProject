using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Orders.DAL.Repositories.Interfaces;
using Orders.Domain.Enums;
using Orders.Domain.Models;

namespace Orders.DAL.Repositories.Implementations
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public OrderRepository(DbConnection connection, DbTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
        }

        public async Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT * FROM orders WHERE order_id = @Id",
                new { Id = orderId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return await Connection.QuerySingleOrDefaultAsync<Order?>(cmd);
        }

        public async Task<List<Order>> GetOrdersAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var skip = (pageNumber - 1) * pageSize;

            var cmd = new CommandDefinition(
                "SELECT * FROM orders OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY",
                new { Skip = skip, Take = pageSize },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return (await Connection.QueryAsync<Order>(cmd)).ToList();
        }

        public async Task<Order?> GetOrderByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT * FROM orders WHERE customer_id = @Id",
                new { Id = customerId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return await Connection.QuerySingleOrDefaultAsync<Order?>(cmd);
        }

        public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition("create_order",
                new
                {
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.ExecuteAsync(cmd);
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Guid orderId, Order order, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "UPDATE orders SET total_amount = @TotalAmount WHERE order_id = @Id",
                new { Id = orderId, TotalAmount = order.TotalAmount },
                cancellationToken: cancellationToken,
                transaction: Transaction
            );

            await Connection.ExecuteAsync(cmd);
            return order;
        }

        public async Task<Order> UpdateOrderStatusAsync(Guid orderId, Order order, Status status, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "UPDATE orders SET status = @Status WHERE order_id = @Id",
                new { Id = orderId, Status = status },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.ExecuteAsync(cmd);
            return order;
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "DELETE FROM orders WHERE order_id = @Id",
                new { Id = orderId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            var rowsDeleted = await Connection.ExecuteAsync(cmd);
            return rowsDeleted == 1; 
        }
    }
}