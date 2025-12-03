using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Repositories;
using SocialAndReviews.Infrastructure.Database;

namespace SocialAndReviews.Infrastructure.Repositories
{
    public class UserProfileRepository : MongoRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(SocialAndReviewsDbContext context) : base(context, "UserProfiles")
        {
        }

        public async Task<bool> IsNicknameUniqueAsync(string nickname, CancellationToken cancellationToken = default)
        {
            // Case-insensitive перевірка, якщо потрібно, або точний збіг
            // Тут використовуємо точний збіг, покладаючись на унікальний індекс БД
            var filter = Builders<UserProfile>.Filter.Eq(u => u.Nickname, nickname);

            var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            return count == 0;
        }
    }
}