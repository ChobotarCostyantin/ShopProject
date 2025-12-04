using MongoDB.Driver;
using Shared.DTOs;
using SocialAndReviews.Domain.Common;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Repositories;
using SocialAndReviews.Infrastructure.Database;

namespace SocialAndReviews.Infrastructure.Repositories
{
    public class ReviewRepository : MongoRepository<Review>, IReviewRepository
    {
        public ReviewRepository(SocialAndReviewsDbContext context) : base(context, "Reviews")
        {
        }

        public async Task<PaginationResult<Review>> GetReviewsByProductIdAsync(Guid productId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var entities = await _collection
                .Find(r => r.ProductId == productId)
                .Sort(Builders<Review>.Sort.Ascending(r => r.Id))
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = CountAllAsync(_ => true, cancellationToken).Result;

            return PaginationResult<Review>.Create(
                entities.ToArray(),
                totalCount,
                pageNumber,
                pageSize);
        }

        public async Task<PaginationResult<Review>> GetReviewsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var entities = await _collection
                .Find(_ => true)
                .Sort(Builders<Review>.Sort.Ascending(r => r.Id))
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = await CountAllAsync(_ => true, cancellationToken);

            return PaginationResult<Review>.Create(
                entities.ToArray(),
                totalCount,
                pageNumber,
                pageSize);
        }

        public async Task<IEnumerable<Review>> SearchByTextAsync(string searchText, int limit, CancellationToken cancellationToken)
        {
            return await _collection.Find(Builders<Review>.Filter.Text(searchText))
                .Sort(Builders<Review>.Sort.MetaTextScore("textScore"))
                .Limit(limit)
                .ToListAsync(cancellationToken);
        }

        public async Task<(double AverageRating, long TotalCount)> GetProductRatingSummaryAsync(Guid productId, CancellationToken cancellationToken)
        {
            var result = await _collection.Aggregate()
                .Match(r => r.ProductId == productId)
                .Group(
                    key => key.ProductId,
                    g => new
                    {
                        AverageRating = g.Average(r => r.Rating.Value),
                        TotalCount = g.Count()
                    }
                )
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                return (0, 0);
            }

            return (result.AverageRating, result.TotalCount);
        }
    }
}