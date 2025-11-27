using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
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

        // Хелпер-метод для ручного маппінгу DbDataReader в об'єкт Customer
        private static Customer MapCustomer(DbDataReader reader)
        {
            return new Customer
            {
                // Примітка: використовуємо GetString та GetGuid за іменем колонки
                CustomerId = reader.GetGuid(reader.GetOrdinal("customer_id")),
                FullName = reader.GetString(reader.GetOrdinal("full_name")),
                Email = reader.GetString(reader.GetOrdinal("email"))
            };
        }

        public async Task<Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            const string sql = "SELECT customer_id, full_name, email FROM customers WHERE customer_id = @Id";

            await using var command = Connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = Transaction;

            var idParam = command.CreateParameter();
            idParam.ParameterName = "@Id";
            idParam.Value = customerId;
            command.Parameters.Add(idParam);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return MapCustomer(reader);
            }

            return null;
        }

        public async Task<List<Customer>> GetCustomersAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            var customers = new List<Customer>();
            var skip = (pageNumber - 1) * pageSize;

            // PostgreSQL синтаксис для пагінації (OFFSET/FETCH)
            const string sql = "SELECT customer_id, full_name, email FROM customers OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";

            await using var command = Connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = Transaction;

            var skipParam = command.CreateParameter();
            skipParam.ParameterName = "@Skip";
            skipParam.Value = skip;
            skipParam.DbType = DbType.Int32;
            command.Parameters.Add(skipParam);

            var takeParam = command.CreateParameter();
            takeParam.ParameterName = "@Take";
            takeParam.Value = pageSize;
            takeParam.DbType = DbType.Int32;
            command.Parameters.Add(takeParam);

            // 4. Ітерація по результатам і маппінг
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                customers.Add(MapCustomer(reader));
            }

            return customers;
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            // Виклик збереженої процедури 'create_customer'
            const string spName = "create_customer";

            await using var command = Connection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure; // Важливо вказати тип команди
            command.Transaction = Transaction;

            // Налаштування параметрів процедури
            var idParam = command.CreateParameter();
            idParam.ParameterName = "p_customer_id";
            idParam.Value = customer.CustomerId;
            command.Parameters.Add(idParam);

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "p_full_name";
            nameParam.Value = customer.FullName;
            nameParam.DbType = DbType.String;
            command.Parameters.Add(nameParam);

            var emailParam = command.CreateParameter();
            emailParam.ParameterName = "p_email";
            emailParam.Value = customer.Email;
            emailParam.DbType = DbType.String;
            command.Parameters.Add(emailParam);

            // 5. Виконання процедури (без повернення даних)
            await command.ExecuteNonQueryAsync(cancellationToken);
            return customer;
        }

        public async Task<Customer> UpdateCustomerAsync(Guid customerId, Customer customer, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            const string sql = "UPDATE customers SET full_name = @FullName WHERE customer_id = @Id";

            await using var command = Connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = Transaction;

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "@FullName";
            nameParam.Value = customer.FullName;
            nameParam.DbType = DbType.String;
            command.Parameters.Add(nameParam);

            var idParam = command.CreateParameter();
            idParam.ParameterName = "@Id";
            idParam.Value = customerId;
            command.Parameters.Add(idParam);

            await command.ExecuteNonQueryAsync(cancellationToken);
            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(Guid customerId, CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            const string sql = "DELETE FROM customers WHERE customer_id = @Id";

            await using var command = Connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = Transaction;

            var idParam = command.CreateParameter();
            idParam.ParameterName = "@Id";
            idParam.Value = customerId;
            command.Parameters.Add(idParam);

            // 6. ExecuteNonQueryAsync повертає кількість змінених рядків
            var rowsDeleted = await command.ExecuteNonQueryAsync(cancellationToken);
            return rowsDeleted == 1;
        }

        public async Task<long> CountAllAsync(CancellationToken cancellationToken)
        {
            ThrowIfConnectionOrTransactionIsUninitialized();

            const string sql = "SELECT COUNT(*) FROM customers";

            await using var command = Connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = Transaction;

            var result = await command.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt64(result);
        }
    }
}