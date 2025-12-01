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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogDbContext dbContext) : base(dbContext) { }

        public async Task<Category?> GetCategoryWithProductsAsync(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories
                .Where(c => c.CategoryId == categoryId)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}