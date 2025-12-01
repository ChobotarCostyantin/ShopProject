using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL.Repositories.Interfaces;

namespace Catalog.DAL.UOW.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IProductDetailsRepository ProductDetailsRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductTagRepository ProductTagRepository { get; }
        ITagRepository TagRepository { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}