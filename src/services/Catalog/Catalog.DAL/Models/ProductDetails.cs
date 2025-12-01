using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.DAL.Models
{
    public class ProductDetails
    {
        public Guid DetailsId { get; set; }
        public Guid ProductId { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        public float Weight_Kg { get; set; }

        public Product Product { get; set; } = null!;

        public uint RowVersion { get; set; }
    }
}