using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Orders.BLL.Features.Customers.Services.Implementations;
using Orders.BLL.Features.Customers.Services.interfaces;
using Orders.BLL.Features.Orders.Services.Implementations;
using Orders.BLL.Features.Orders.Services.Interfaces;
using Orders.BLL.Features.OrderItems.Services.Implementations;
using Orders.BLL.Features.OrderItems.Services.Interfaces;
using Orders.BLL.Features.OrderShipping.Services.Implementations;
using Orders.BLL.Features.OrderShipping.Services.Interfaces;

namespace Orders.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureBusinessLayer(this IServiceCollection services)
        {
            var currentAssembly = typeof(DependencyInjection).Assembly;
            services.AddValidatorsFromAssembly(currentAssembly);

            services.AddAutoMapper(cfg => {}, currentAssembly);

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IOrderShippingService, OrderShippingService>();

            return services;
        }
    }
}