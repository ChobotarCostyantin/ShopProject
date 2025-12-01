using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DAL.Database
{
    public class DatabaseSeeder
    {
        public static void SeedDatabase(ModelBuilder modelBuilder)
        {
            SeedCategory(modelBuilder);
            SeedProduct(modelBuilder);
        }
        private static void SeedCategory(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Category>()
                .HasData(new Category
                    {
                        CategoryId = new Guid("b3e7c2e7-6b7b-4b7b-9b7b-7b7b7b7b7b7b"),
                        Name = "Phones"
                    },
                    new Category
                    {
                        CategoryId = new Guid("b3e7c2e7-6b7b-4b7b-9b7b-7b7b7b7b7b7c"),
                        Name = "Laptops"
                    },
                    new Category
                    {
                        CategoryId = new Guid("b3e7c2e7-6b7b-4b7b-9b7b-7b7b7b7b7b7d"),
                        Name = "Tablets"
                    },
                    new Category
                    {
                        CategoryId = new Guid("b3e7c2e7-6b7b-4b7b-9b7b-7b7b7b7b7b7e"),
                        Name = "Monitors"
                    },
                    new Category
                    {
                        CategoryId = new Guid("b3e7c2e7-6b7b-4b7b-9b7b-7b7b7b7b7b7f"),
                        Name = "Accessories"
                    }
                    );
        }

        private static void SeedProduct(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasData(new Product
                {
                    ProductId = new Guid("11111111-1111-1111-1111-111111111111"),
                    CategoryId = new Guid("b3e7c2e7-6b7b-4b7b-9b7b-7b7b7b7b7b7b"),
                    Name = "iPhone X",
                    Sku = "iphonex",
                    Price = 21000,
                    StockQuantity = 20,
                    ReorderLevel = 5
                },
                new Product
                {
                    ProductId = new Guid("22222222-2222-2222-2222-222222222222"),
                    CategoryId = new Guid("b3e7c2e7-6b7b-4b7b-9b7b-7b7b7b7b7b7c"),
                    Name = "MacBook Pro 13",
                    Sku = "macbookpro13",
                    Price = 50000,
                    StockQuantity = 10,
                    ReorderLevel = 3
                },
                new Product
                {
                    ProductId = new Guid("33333333-3333-3333-3333-333333333333"),
                    CategoryId = new Guid("b3e7c2e7-6b7b-4b7b-9b7b-7b7b7b7b7b7d"),
                    Name = "iPad Pro 11",
                    Sku = "ipadpro11",
                    Price = 30000,
                    StockQuantity = 5,
                    ReorderLevel = 3
                }
                );
        }
    }
}