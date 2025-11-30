using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.DAL.Database.ConnectionAccessor;
using Orders.DAL.Repositories;
using Orders.DAL.Repositories.Implementations;
using Orders.DAL.Repositories.Interfaces;
using Orders.DAL.Repositories.UOW.Implementations;
using Orders.DAL.Repositories.UOW.Interfaces;
using Shared.Exceptions;

namespace Orders.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            services.AddScoped<IDatabaseConnectionAccessor, NpgsqlConnectionAccessor>(_ => new NpgsqlConnectionAccessor(
                configuration.GetConnectionString("ordersDb")
                ?? throw new ItemInConfigurationNotFoundException(IDatabaseConnectionAccessor.DatabaseConnectionConfigurationKey)));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderShippingRepository, OrderShippingRepository>();

            services.AddScoped<RepositoryBase, CustomerRepository>();
            services.AddScoped<RepositoryBase, OrderRepository>();
            services.AddScoped<RepositoryBase, OrderItemRepository>();
            services.AddScoped<RepositoryBase, OrderShippingRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}