using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Orders.DAL.Repositories.Interfaces;
using Orders.Domain.Models;

namespace Orders.DAL.Repositories.Implementations
{
    public class OrderItemRepository : RepositoryBase, IOrderItemRepository
    {
        public OrderItemRepository(DbConnection connection, DbTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
        }

        public async Task<OrderItem?> GetOrderItemAsync(Guid orderItemId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT * FROM order_items WHERE order_item_id = @Id",
                new { Id = orderItemId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return await Connection.QuerySingleOrDefaultAsync<OrderItem?>(cmd);
        }

        public async Task<List<OrderItem>> GetOrderItemsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var skip = (pageNumber - 1) * pageSize;

            var cmd = new CommandDefinition(
                "SELECT * FROM order_items OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY",
                new { Skip = skip, Take = pageSize },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return (await Connection.QueryAsync<OrderItem>(cmd)).ToList();
        }

        public async Task<List<OrderItem>?> GetOrderItemsByOrderIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT * FROM order_items WHERE order_id = @Id",
                new { Id = orderId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return (await Connection.QueryAsync<OrderItem>(cmd)).ToList();
        }

        public async Task<OrderItem> CreateOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition("create_order_item",
                new
                {
                    OrderItemId = orderItem.OrderItemId,
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.ExecuteAsync(cmd);
            return orderItem;
        }

        public async Task<OrderItem> UpdateOrderItemAsync(Guid orderItemId, OrderItem orderItem, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "UPDATE order_items SET quantity = @Quantity WHERE order_item_id = @Id",
                new { Id = orderItemId, Quantity = orderItem.Quantity },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.ExecuteAsync(cmd);
            return orderItem;
        }

        public async Task<bool> DeleteOrderItemAsync(Guid orderItemId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "DELETE FROM order_items WHERE order_item_id = @Id",
                new { Id = orderItemId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            var rowsDeleted = await Connection.ExecuteAsync(cmd);
            return rowsDeleted == 1; 
        }

        public async Task<long> CountOrderItemsAsync(Guid orderId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT COUNT(*) FROM order_items WHERE order_id = @Id",
                new { Id = orderId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return await Connection.ExecuteScalarAsync<long>(cmd);
        }
    }
}