using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTOs;
using SocialAndReviews.Domain.Entities;

namespace SocialAndReviews.Domain.Interfaces.Repositories
{
    public interface IReviewRepository : IMongoRepository<Review>
    {
        Task<PaginationResult<Review>> GetReviewsByProductIdAsync(Guid productId, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<PaginationResult<Review>> GetReviewsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        // Пошук по тексту (вже є)
        Task<IEnumerable<Review>> SearchByTextAsync(string searchText, int limit, CancellationToken cancellationToken);

        // [НОВЕ] Агрегація: Розрахунок середнього рейтингу та кількості відгуків
        // Це неможливо ефективно зробити через Find(), тому Aggregate тут необхідний.
        Task<(double AverageRating, long TotalCount)> GetProductRatingSummaryAsync(Guid productId, CancellationToken cancellationToken);
    }
}