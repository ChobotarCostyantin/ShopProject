using System.Data.Common;
using Npgsql;

namespace Orders.DAL.Database.ConnectionAccessor
{
    public class NpgsqlConnectionAccessor : IDatabaseConnectionAccessor
    {
        private readonly string _connectionString;

        public NpgsqlConnectionAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}