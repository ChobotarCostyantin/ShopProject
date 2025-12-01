using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.DAL.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public ICollection<Product> Products { get; set; } = [];
    }
}