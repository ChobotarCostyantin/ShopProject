using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Catalog.BLL.Services.Implementations;
using Catalog.BLL.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureBusinessLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(_ => { }, Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductDetailsService, ProductDetailsService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITagService, TagService>();

            return services;
        }
    }
}