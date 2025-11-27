using System.Data.Common;

namespace Orders.DAL.Database.ConnectionAccessor
{
    public interface IDatabaseConnectionAccessor
    {
        const string DatabaseConnectionConfigurationKey = "Default";
        DbConnection GetConnection();
    }
}