using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialAndReviews.Domain.Entities;

namespace SocialAndReviews.Domain.Interfaces.Repositories
{
    public interface IUserProfileRepository : IMongoRepository<UserProfile>
    {
        // Швидкий пошук по зовнішньому ID (Guid)
        // Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        // Перевірка унікальності нікнейму (важливо для eventual consistency)
        Task<bool> IsNicknameUniqueAsync(string nickname, CancellationToken cancellationToken = default);
    }

}