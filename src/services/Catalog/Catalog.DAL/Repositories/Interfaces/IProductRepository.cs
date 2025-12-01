using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL.Models;

namespace Catalog.DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetProductWithRelatedEntitiesAsync(Guid productId, bool includeProductDetails, bool includeProductTags, bool includeCategory, CancellationToken cancellationToken);
    }
}