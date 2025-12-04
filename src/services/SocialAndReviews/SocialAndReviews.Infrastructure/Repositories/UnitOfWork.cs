using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialAndReviews.Domain.Interfaces.Repositories;
using SocialAndReviews.Infrastructure.Database;

namespace SocialAndReviews.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialAndReviewsDbContext _context;

        public IReviewRepository ReviewRepository { get; }
        public IUserProfileRepository UserProfileRepository { get; }

        public UnitOfWork(SocialAndReviewsDbContext context, IReviewRepository reviewRepository, IUserProfileRepository userProfileRepository)
        {
            _context = context;
            ReviewRepository = reviewRepository;
            UserProfileRepository = userProfileRepository;
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (_context.Session != null && _context.Session.IsInTransaction)
            {
                try
                {
                    await _context.Session.CommitTransactionAsync(cancellationToken);
                    return true;
                }
                catch
                {
                    await _context.Session.AbortTransactionAsync(cancellationToken);
                    throw;
                }
                finally
                {
                    _context.Session.Dispose();
                    _context.Session = null;
                }
            }

            // Якщо транзакція не була відкрита явно, зміни вже записані (MongoDB default behavior per operation)
            return true;
        }

        public void Dispose()
        {
            _context.Session?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}