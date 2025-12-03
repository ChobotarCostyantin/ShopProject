using MongoDB.Driver;
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

        public async Task<IEnumerable<Review>> SearchByTextAsync(string searchText, int limit, CancellationToken cancellationToken = default)
        {
            return await _collection.Aggregate()
                .Match(Builders<Review>.Filter.Text(searchText))
                .Sort(Builders<Review>.Sort.MetaTextScore("textScore")) // Сортування за якістю збігу
                .Limit(limit)
                .ToListAsync(cancellationToken);
        }

        public async Task<(double AverageRating, long TotalCount)> GetProductRatingSummaryAsync(Guid productId, CancellationToken cancellationToken = default)
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