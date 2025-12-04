using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Shared.DTOs;
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
        public async Task<PaginationResult<UserProfile>> GetUserProfilesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var entities = await _collection
                .Find(_ => true)
                .Sort(Builders<UserProfile>.Sort.Ascending(r => r.Id))
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = CountAllAsync(_ => true, cancellationToken).Result;

            return PaginationResult<UserProfile>.Create(
                entities.ToArray(),
                totalCount,
                pageNumber,
                pageSize);
        }

        public async Task<bool> IsNicknameAvailableForUpdateAsync(string nickname, Guid currentUserId, CancellationToken cancellationToken)
        {
            var nicknameFilter = Builders<UserProfile>.Filter.Eq(x => x.Nickname, nickname);

            var idExclusionFilter = Builders<UserProfile>.Filter.Ne(x => x.Id, currentUserId);

            var finalFilter = Builders<UserProfile>.Filter.And(nicknameFilter, idExclusionFilter);

            var conflictingProfile = await _collection.Find(finalFilter).FirstOrDefaultAsync(cancellationToken);

            return conflictingProfile is null;
        }

        public async Task<bool> IsNicknameUniqueAsync(string nickname, CancellationToken cancellationToken)
        {
            // Case-insensitive перевірка, якщо потрібно, або точний збіг
            // Тут використовуємо точний збіг, покладаючись на унікальний індекс БД
            var filter = Builders<UserProfile>.Filter.Eq(u => u.Nickname, nickname);

            var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            return count == 0;
        }
    }
}