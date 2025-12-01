using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.DAL.Models
{
    public class Tag
    {
        public Guid TagId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<ProductTag> ProductTags { get; set; } = [];
    }
}