using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL.Database;
using Catalog.DAL.Models;
using Catalog.DAL.Repositories.Implementations;
using Catalog.DAL.Repositories.Interfaces;
using Catalog.DAL.UOW.Implementations;
using Catalog.DAL.UOW.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shared.Exceptions;

namespace Catalog.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString(
                    CatalogDbContext.ConnectionStringConfigurationKey
                ) ?? throw new ItemInConfigurationNotFoundException(CatalogDbContext.ConnectionStringConfigurationKey));
                opt.UseSnakeCaseNamingConvention();

                var tagId1 = Guid.Parse("88888888-8888-8888-8888-888888888888");
                var tagId2 = Guid.Parse("adcdcdcd-cdcd-cdcd-cdcd-cdcdcdcdcdcd");
                var tagId3 = Guid.Parse("abababab-abab-abab-abab-abababababab");
                var tagId4 = Guid.Parse("acacacac-acac-acac-acac-acacacacacac");

                opt.UseSeeding((context, _) =>
                {
                    if(!context.Set<Tag>().Any())
                    {
                        var tagList = new []
                        {
                            new Tag { TagId = tagId1, Name = "new" },
                            new Tag { TagId = tagId2, Name = "hot" },
                            new Tag { TagId = tagId3, Name = "2025" },
                            new Tag { TagId = tagId4, Name = "phone case" }
                        };

                        context.Set<Tag>().AddRange(tagList);
                        context.SaveChanges();
                    }
                });

                opt.UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    if(!await context.Set<Tag>().AnyAsync(cancellationToken))
                    {
                        var tagList = new []
                        {
                            new Tag { TagId = tagId1, Name = "new" },
                            new Tag { TagId = tagId2, Name = "hot" },
                            new Tag { TagId = tagId3, Name = "2025" },
                            new Tag { TagId = tagId4, Name = "phone case" }
                        };

                        await context.Set<Tag>().AddRangeAsync(tagList, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                });
            });

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductDetailsRepository, ProductDetailsRepository>();
            services.AddScoped<IProductTagRepository, ProductTagRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}