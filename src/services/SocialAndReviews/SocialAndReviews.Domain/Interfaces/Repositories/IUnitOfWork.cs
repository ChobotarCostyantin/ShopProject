using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
{
    IReviewRepository ReviewRepository { get; }
    IUserProfileRepository UserProfileRepository { get; }

    // Зберігає зміни. У контексті Mongo це може бути коміт транзакції 
    // або просто заглушка, якщо репозиторії зберігають дані миттєво.
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
}