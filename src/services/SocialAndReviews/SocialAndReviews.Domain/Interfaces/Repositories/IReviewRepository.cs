using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialAndReviews.Domain.Entities;

namespace SocialAndReviews.Domain.Interfaces.Repositories
{
    public interface IReviewRepository : IMongoRepository<Review>
    {
        // Стандартна пагінація
        // Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId, int page, int pageSize, CancellationToken cancellationToken = default);

        // MongoDB Text Search: Пошук по індексованому текстовому полю (Full-text search)
        Task<IEnumerable<Review>> SearchByTextAsync(string searchText, int limit, CancellationToken cancellationToken = default);

        // MongoDB Aggregation: Розрахунок середнього рейтингу та кількості відгуків
        // Повертає Value Object або DTO з результатами агрегації
        // Task<(double AverageRating, long TotalCount)> GetProductRatingSummaryAsync(Guid productId, CancellationToken cancellationToken = default);

        // // MongoDB Aggregation: Отримання розподілу оцінок (наприклад: 5 зірок - 100, 4 зірки - 20...)
        // Task<Dictionary<int, long>> GetRatingDistributionAsync(Guid productId, CancellationToken cancellationToken = default);
    }
}