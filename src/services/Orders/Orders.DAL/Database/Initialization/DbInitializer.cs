using Orders.DAL.Database.ConnectionAccessor;
using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Shared.Exceptions;

namespace Orders.DAL.Database.Initialization
{
    public static class DbInitializer
    {
        public static Task MigrateDatabase(this WebApplication app)
        {
            var dbConnection = app.Configuration.GetConnectionString("orderDb");

            if (string.IsNullOrWhiteSpace(dbConnection))
            {
                throw new ItemInConfigurationNotFoundException(IDatabaseConnectionAccessor.DatabaseConnectionConfigurationKey);
            }

            EnsureDatabase.For.PostgresqlDatabase(dbConnection);

            var upgrader = DeployChanges.To.PostgresqlDatabase(dbConnection)
                .WithScriptsEmbeddedInAssembly(typeof(DbInitializer).Assembly)
                .LogToConsole()
                .Build();

            if (upgrader.IsUpgradeRequired())
            {
                upgrader.PerformUpgrade();
            }

            return Task.CompletedTask;
        }
    }
}