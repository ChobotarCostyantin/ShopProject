using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL.Database;
using Catalog.DAL.Models;
using Catalog.DAL.Repositories.Interfaces;

namespace Catalog.DAL.Repositories.Implementations
{
    public class ProductDetailsRepository : GenericRepository<ProductDetails>, IProductDetailsRepository
    {
        public ProductDetailsRepository(CatalogDbContext dbContext) : base(dbContext) { }
    }
}