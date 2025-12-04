using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTOs;
using SocialAndReviews.Domain.Entities;

namespace SocialAndReviews.Domain.Interfaces.Repositories
{
    public interface IUserProfileRepository : IMongoRepository<UserProfile>
    {
        Task<PaginationResult<UserProfile>> GetUserProfilesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<bool> IsNicknameAvailableForUpdateAsync(string nickname, Guid currentUserId, CancellationToken cancellationToken);
        Task<bool> IsNicknameUniqueAsync(string nickname, CancellationToken cancellationToken);
    }

}