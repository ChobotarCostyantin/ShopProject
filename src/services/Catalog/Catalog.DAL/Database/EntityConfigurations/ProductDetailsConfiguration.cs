using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DAL.Database.EntityConfigurations
{
    public class ProductDetailsConfiguration : IEntityTypeConfiguration<ProductDetails>
    {
        public void Configure(EntityTypeBuilder<ProductDetails> builder)
        {
            builder.ToTable("product_details");

            builder.HasKey(x => x.DetailsId);

            builder.HasIndex(x => x.ProductId)
                .IsUnique();

            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("TEXT")
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.Manufacturer)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.ToTable(x => 
                x.HasCheckConstraint("ck_weight_kg_greater_than_zero", "weight_kg > 0"));
                
            builder.HasOne(x => x.Product)
                .WithOne(x => x.ProductDetails)
                .HasForeignKey<ProductDetails>(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.RowVersion)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}