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
    public class CustomerRepository : RepositoryBase, ICustomerRepository
    {
        public CustomerRepository(DbConnection connection, DbTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
        }

        public async Task<Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "SELECT * FROM customers WHERE customer_id = @Id",
                new { Id = customerId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return await Connection.QuerySingleOrDefaultAsync<Customer?>(cmd);
        }

        public async Task<List<Customer>> GetCustomersAsync(int pageSize, int pageNumber,CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var skip = (pageNumber - 1) * pageSize;

            var cmd = new CommandDefinition(
                "SELECT * FROM customers OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY",
                new { Skip = skip, Take = pageSize },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            return (await Connection.QueryAsync<Customer>(cmd)).ToList();
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition("create_customer",
                new
                {
                    CustomerId = customer.CustomerId,
                    FullName = customer.FullName,
                    Email = customer.Email
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.ExecuteAsync(cmd);
            return customer;
        }

        public async Task<Customer> UpdateCustomerAsync(Guid customerId, Customer customer, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "UPDATE customers SET full_name = @FullName WHERE customer_id = @Id",
                new { Id = customerId, FullName = customer.FullName },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            await Connection.ExecuteAsync(cmd);
            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var cmd = new CommandDefinition(
                "DELETE FROM customers WHERE customer_id = @Id",
                new { Id = customerId },
                cancellationToken: cancellationToken,
                transaction: Transaction);

            var rowsDeleted = await Connection.ExecuteAsync(cmd);
            return rowsDeleted == 1; 
        }
    }
}