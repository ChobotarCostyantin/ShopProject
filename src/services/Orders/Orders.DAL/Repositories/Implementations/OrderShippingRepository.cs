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
    public class OrderShippingRepository : RepositoryBase, IOrderShippingRepository
    {
        public OrderShippingRepository(DbConnection? connection = null, DbTransaction? transaction = null)
        {
            Connection = connection;
            Transaction = transaction;
        }

        public async Task<OrderShipping?> GetOrderShippingAsync(Guid shippingId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT * FROM order_shippings WHERE shipping_id = @Id",
                new { Id = shippingId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return await Connection.QuerySingleOrDefaultAsync<OrderShipping?>(cmd);
        }

        public async Task<OrderShipping?> GetOrderShippingByOrderIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT * FROM order_shippings WHERE order_id = @Id",
                new { Id = orderId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return await Connection.QuerySingleOrDefaultAsync<OrderShipping?>(cmd);
        }

        public async Task<OrderShipping> CreateOrderShippingAsync(OrderShipping orderShipping, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition("create_order_shipping",
                new
                {
                    p_shipping_id = orderShipping.ShippingId,
                    p_order_id = orderShipping.OrderId,
                    p_adress_line = orderShipping.AdressLine,
                    p_city = orderShipping.City,
                    p_postal_code = orderShipping.PostalCode
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.ExecuteAsync(cmd);
            return orderShipping;
        }

        public async Task<OrderShipping> UpdateOrderShippingAsync(Guid shippingId, OrderShipping orderShipping, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "UPDATE order_shippings SET adress_line = @AdressLine, city = @City, postal_code = @PostalCode WHERE shipping_id = @Id",
                new { Id = shippingId, AdressLine = orderShipping.AdressLine, City = orderShipping.City, PostalCode = orderShipping.PostalCode },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.ExecuteAsync(cmd);
            return orderShipping;
        }

        public async Task<bool> DeleteOrderShippingAsync(Guid shippingId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "DELETE FROM order_shippings WHERE shipping_id = @Id",
                new { Id = shippingId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            var rowsDeleted = await Connection.ExecuteAsync(cmd);
            return rowsDeleted == 1; 
        }
    }
}