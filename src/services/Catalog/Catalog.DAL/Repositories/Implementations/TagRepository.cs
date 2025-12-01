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
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(CatalogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tag>> GetTagsByCategoryNameAsync(string categoryName, CancellationToken cancellationToken)
        {
            var query = from tag in _dbContext.Tags
                        join pt in _dbContext.ProductTags on tag.TagId equals pt.TagId
                        join p in _dbContext.Products on pt.ProductId equals p.ProductId
                        join c in _dbContext.Categories on p.CategoryId equals c.CategoryId
                        where c.Name == categoryName
                        select tag;

            return await query.Distinct().ToListAsync(cancellationToken);
        }
    }
}