using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL.Database;
using Catalog.DAL.Models;
using Catalog.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DAL.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(CatalogDbContext dbContext) : base(dbContext) { }

        public async Task<Product?> GetProductWithRelatedEntitiesAsync(Guid productId, bool includeCategory, bool includeProductDetail, bool includeProductTags, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Where(p => p.ProductId == productId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (product == null) return null;

            if (includeCategory)
            {
                await _dbContext.Entry(product)
                    .Reference(x => x.Category)
                    .LoadAsync(cancellationToken);
            }

            if (includeProductDetail)
            {
                await _dbContext.Entry(product)
                    .Reference(x => x.ProductDetails)
                    .LoadAsync(cancellationToken);
            }

            if (includeProductTags)
            {
                await _dbContext.Entry(product)
                    .Collection(x => x.ProductTags)
                    .Query()
                    .Include(pt => pt.Tag)
                    .LoadAsync(cancellationToken);
            }

            return product;
        }
    }
}