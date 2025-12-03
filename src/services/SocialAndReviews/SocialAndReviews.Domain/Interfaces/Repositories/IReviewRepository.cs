using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialAndReviews.Domain.Entities;

namespace SocialAndReviews.Domain.Interfaces.Repositories
{
    public interface IReviewRepository : IMongoRepository<Review>
    {
        // Пошук по тексту (вже є)
        Task<IEnumerable<Review>> SearchByTextAsync(string searchText, int limit, CancellationToken cancellationToken = default);

        // [НОВЕ] Агрегація: Розрахунок середнього рейтингу та кількості відгуків
        // Це неможливо ефективно зробити через Find(), тому Aggregate тут необхідний.
        Task<(double AverageRating, long TotalCount)> GetProductRatingSummaryAsync(Guid productId, CancellationToken cancellationToken = default);
    }
}