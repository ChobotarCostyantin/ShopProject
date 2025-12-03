using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SocialAndReviews.Domain.Interfaces.Repositories;
using MongoDB.Driver;
using SocialAndReviews.Domain.Common;
using SocialAndReviews.Infrastructure.Database;

namespace SocialAndReviews.Infrastructure.Repositories
{
    public abstract class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> _collection;

        protected MongoRepository(SocialAndReviewsDbContext context, string collectionName)
        {
            _collection = context.Client.GetDatabase(context.Reviews.Database.DatabaseNamespace.DatabaseName)
                .GetCollection<T>(collectionName);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            return await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            return await _collection.Find(_ => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            return await _collection.Find(predicate)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<long> CountAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _collection.CountDocumentsAsync(predicate, cancellationToken: cancellationToken);
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(entity, DatabaseShared.EmptyInsertOneOptions(), cancellationToken);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await _collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
        }

        public virtual async Task RemoveAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, entity.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            var ids = entities.Select(e => e.Id);
            var filter = Builders<T>.Filter.In(doc => doc.Id, ids);
            await _collection.DeleteManyAsync(filter);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, entity.Id);
            
            // Використовуємо ReplaceOne для повного оновлення документа
            // Це ефективно працює з DDD, оскільки ми зберігаємо стан агрегата повністю
            await _collection.ReplaceOneAsync(filter, entity);
        }
    }
}