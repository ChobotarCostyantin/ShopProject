using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.DAL.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Category Category { get; set; } = null!;
        public ProductDetails ProductDetails { get; set; } = null!;
        public ICollection<ProductTag> ProductTags { get; set; } = [];


        public uint RowVersion { get; set; }
    }
}