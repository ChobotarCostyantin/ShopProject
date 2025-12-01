using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DAL.Database.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(x => x.ProductId);

            builder.Property(x => x.CategoryId);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(x => x.Sku)
                .IsUnique();

            builder.Property(x => x.Sku)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.ToTable(
                x => x.HasCheckConstraint("ck_price_greater_than_zero", "price > 0"));

            builder.Property(x => x.StockQuantity)
                .HasDefaultValue(0);

            builder.ToTable(
                x => x.HasCheckConstraint("ck_stock_quantity_greater_than_zero", "stock_quantity > 0"));

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProductDetails)
                .WithOne(x => x.Product)
                .HasForeignKey<ProductDetails>(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.ProductTags)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            builder.Property(x => x.RowVersion)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}