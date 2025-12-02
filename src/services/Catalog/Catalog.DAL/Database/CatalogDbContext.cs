using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Catalog.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DAL.Database
{
    public class CatalogDbContext : DbContext
    {
        public const string ConnectionStringConfigurationKey = "catalogDb";

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductDetails> ProductDetails => Set<ProductDetails>();
        public DbSet<ProductTag> ProductTags => Set<ProductTag>();
        public DbSet<Tag> Tags => Set<Tag>();

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            DatabaseSeeder.SeedDatabase(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}