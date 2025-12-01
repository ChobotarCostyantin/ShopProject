using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL.Database;
using Catalog.DAL.Repositories.Interfaces;
using Catalog.DAL.UOW.Interfaces;

namespace Catalog.DAL.UOW.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogDbContext _dbContext;

        public ICategoryRepository CategoryRepository { get; }
        public IProductDetailsRepository ProductDetailsRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IProductTagRepository ProductTagRepository { get; }
        public ITagRepository TagRepository { get; }

        public UnitOfWork(CatalogDbContext dbContext, ICategoryRepository categoryRepository,
            IProductDetailsRepository productDetailsRepository, IProductRepository productRepository,
            IProductTagRepository productTagRepository, ITagRepository tagRepository)
        {
            _dbContext = dbContext;
            CategoryRepository = categoryRepository;
            ProductDetailsRepository = productDetailsRepository;
            ProductRepository = productRepository;
            ProductTagRepository = productTagRepository;
            TagRepository = tagRepository;
        }

        public int SaveChanges() => _dbContext.SaveChanges();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken) => _dbContext.SaveChangesAsync(cancellationToken);
    }
}