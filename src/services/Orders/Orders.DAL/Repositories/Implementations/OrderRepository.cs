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
        public OrderRepository(DbConnection? connection = null, DbTransaction? transaction = null)
        {
            Connection = connection;
            Transaction = transaction;
        }

        private async Task<List<Order>> QueryOrdersAsync(string sql, object parameters, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var orderDictionary = new Dictionary<Guid, Order>();

            var cmd = new CommandDefinition(
                sql,
                parameters,
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.QueryAsync<Order, OrderItem, Order>(
                cmd,
                (order, orderItem) =>
                {
                    if (!orderDictionary.TryGetValue(order.OrderId, out var currentOrder))
                    {
                        currentOrder = order;
                        currentOrder.OrderItems = new List<OrderItem>();
                        orderDictionary.Add(order.OrderId, currentOrder);
                    }

                    if (orderItem != null && orderItem.OrderItemId != Guid.Empty)
                    {
                        currentOrder.OrderItems.Add(orderItem);
                    }

                    return currentOrder;
                },
                splitOn: "OrderItemId");

            return orderDictionary.Values.ToList();
        }


        public async Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
        {
            const string sql = """
                        SELECT
                            o.order_id AS OrderId,
                            o.customer_id AS CustomerId,
                            o.delivery_date AS DeliveryDate,
                            o.total_price AS TotalPrice,
                            o.status AS Status,
                            o.created_at AS CreatedAt,

                            oi.order_item_id AS OrderItemId,
                            oi.order_id AS OrderId,
                            oi.product_id AS ProductId,
                            oi.quantity AS Quantity
                        FROM orders o
                        LEFT JOIN order_items oi ON o.order_id = oi.order_id
                        WHERE o.order_id = @Id
                        """;

            var list = await QueryOrdersAsync(sql, new { Id = orderId }, cancellationToken);
            return list.FirstOrDefault();
        }

        public async Task<List<Order>> GetOrdersAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            var skip = (pageNumber - 1) * pageSize;

            const string sql = """
                        SELECT 
                            o.order_id AS OrderId, o.customer_id AS CustomerId, o.delivery_date AS DeliveryDate,
                            o.total_price AS TotalPrice, o.status AS Status, o.created_at AS CreatedAt,

                            oi.order_item_id AS OrderItemId, oi.order_id AS OrderId, oi.product_id AS ProductId,
                            oi.quantity AS Quantity
                        FROM orders o
                        INNER JOIN (
                            SELECT order_id
                            FROM orders
                            ORDER BY created_at DESC
                            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                        ) AS p ON p.order_id = o.order_id
                        LEFT JOIN order_items oi ON o.order_id = oi.order_id
                        ORDER BY o.created_at DESC, oi.order_item_id
                        """;

            return await QueryOrdersAsync(sql, new { Skip = skip, Take = pageSize }, cancellationToken);
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(Guid customerId, int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            var skip = (pageNumber - 1) * pageSize;

            const string sql = """
                        SELECT 
                            o.order_id AS OrderId, o.customer_id AS CustomerId, o.delivery_date AS DeliveryDate,
                            o.total_price AS TotalPrice, o.status AS Status, o.created_at AS CreatedAt,

                            oi.order_item_id AS OrderItemId, oi.order_id AS OrderId, oi.product_id AS ProductId,
                            oi.quantity AS Quantity
                        FROM orders o
                        INNER JOIN (
                            SELECT order_id
                            FROM orders
                            WHERE customer_id = @CustomerId
                            ORDER BY created_at DESC
                            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                        ) AS p ON p.order_id = o.order_id
                        LEFT JOIN order_items oi ON o.order_id = oi.order_id
                        ORDER BY o.created_at DESC, oi.order_item_id
                        """;

            return await QueryOrdersAsync(
                sql,
                new { CustomerId = customerId, Skip = skip, Take = pageSize },
                cancellationToken);
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
                new
                {
                    Id = orderId,
                    TotalPrice = order.TotalPrice,
                    DeliveryDate = order.DeliveryDate,
                    Status = order.Status.ToString()
                },
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

        public async Task<long> CountAllOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT COUNT(*) FROM orders WHERE customer_id = @Id",
                new { Id = customerId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return await Connection.ExecuteScalarAsync<long>(cmd);
        }

        public async Task<long> CountAllOrdersAsync(CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT COUNT(*) FROM orders",
                cancellationToken: cancellationToken,
                transaction: Transaction
            );

            return await Connection.ExecuteScalarAsync<long>(cmd);
        }
    }
}