using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.DAL.Repositories.Interfaces
{
    public interface IMongoRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageNumber, CancellationToken cancellationToken);

    Task<long> CountAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    Task RemoveAsync(TEntity entity);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities);

    Task UpdateAsync(TEntity entity);
}
}