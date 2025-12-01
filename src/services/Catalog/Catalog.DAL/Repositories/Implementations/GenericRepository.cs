using System.Linq.Expressions;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Catalog.DAL.Repositories.Interfaces;
using Catalog.DAL.Database;

namespace Catalog.DAL.Repositories.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly CatalogDbContext _dbContext;

        public GenericRepository(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TEntity>().FindAsync([id], cancellationToken);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TEntity>().ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<TEntity>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken: cancellationToken);

        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<TEntity>()
                .Where(predicate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<long> CountAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TEntity>().CountAsync(predicate, cancellationToken);
        }

        public async Task<TEntity?> GetBySpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
            => await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> ListBySpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
            => await ApplySpecification(specification).ToListAsync(cancellationToken);

        public async Task<int> CountBySpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
            => await ApplySpecification(specification).CountAsync(cancellationToken);


        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public Task RemoveAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator.Default.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification);
        }

    }
}