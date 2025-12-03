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

        public IReviewRepository Reviews { get; }
        public IUserProfileRepository UserProfiles { get; }

        public UnitOfWork(SocialAndReviewsDbContext context, IReviewRepository reviews, IUserProfileRepository userProfiles)
        {
            _context = context;
            Reviews = reviews;
            UserProfiles = userProfiles;
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
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