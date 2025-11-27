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

            const string sql = """
                               SELECT
                                   -- Order
                                   o.order_id AS OrderId,
                                   o.customer_id AS CustomerId,
                                   o.delivery_date AS DeliveryDate,
                                   o.total_price AS TotalPrice,
                                   o.status AS Status,
                                   o.created_at AS CreatedAt,

                                   -- Order Item
                                   oi.order_item_id AS OrderItemId,
                                   oi.order_id AS OrderId,
                                   oi.product_id AS ProductId,
                                   oi.product_name AS ProductName,
                                   oi.unit_price AS UnitPrice,
                                   oi.quantity AS Quantity,
                               FROM orders o
                               LEFT JOIN order_items oi ON o.order_id = oi.order_id
                               WHERE o.order_id = @Id
                               """;
            var orderDictionary = new Dictionary<Guid, Order>();

            var cmd = new CommandDefinition(sql,
                new { Id = orderId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.QueryAsync<Order, OrderItem, Order>(
                cmd,
                (order, orderItem) =>
                {
                    if (!orderDictionary.TryGetValue(order.OrderId, out var currentOrder))
                    {
                        currentOrder = order;
                        order.OrderItems = [];
                        orderDictionary.Add(order.OrderId, currentOrder);
                    }

                    if (orderItem is not null)
                    {
                        currentOrder.OrderItems.Add(orderItem);
                    }

                    return currentOrder;
                },
                splitOn: "order_item_id");

            return orderDictionary.Values.FirstOrDefault();
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
                    p_order_id = order.OrderId,
                    p_customer_id = order.CustomerId,
                    p_delivery_date = order.DeliveryDate,
                    p_total_price = order.TotalPrice,
                    p_status = order.Status.ToString(),
                    p_created_at = order.CreatedAt
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
                "UPDATE orders SET total_price = @TotalPrice, delivery_date = @DeliveryDate, status = @Status WHERE order_id = @Id",
                new {
                    Id = orderId, 
                    TotalPrice = order.TotalPrice, 
                    DeliveryDate = order.DeliveryDate, 
                    Status = order.Status.ToString() },
                cancellationToken: cancellationToken,
                transaction: Transaction
            );

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

        public async Task<long> CountAllAsync(CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT COUNT(*) FROM orders",
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return await Connection.ExecuteScalarAsync<long>(cmd);
        }
    }
}