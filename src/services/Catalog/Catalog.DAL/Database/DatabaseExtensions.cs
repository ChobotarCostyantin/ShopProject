using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.DAL.Database
{
    public static class DatabaseExtensions
    {
        public static async Task MigrateDatabaseAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();

            await using var DbContext = Scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

            await DbContext.Database.MigrateAsync();
        }
    }
}