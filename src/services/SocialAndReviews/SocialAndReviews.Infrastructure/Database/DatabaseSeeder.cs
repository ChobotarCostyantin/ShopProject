using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.ValueObjects;

namespace SocialAndReviews.Infrastructure.Database
{
    public interface IDataSeeder
    {
        Task SeedAsync();
    }

    public class SocialAndReviewsSeeder : IDataSeeder
    {
        private readonly SocialAndReviewsDbContext _context;
        private readonly ILogger<SocialAndReviewsSeeder> _logger;

        public SocialAndReviewsSeeder(SocialAndReviewsDbContext context, ILogger<SocialAndReviewsSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await SeedUserProfilesAsync();
            await SeedReviewsAsync();
        }

        private async Task SeedUserProfilesAsync()
        {
            // Ідемпотентність: перевіряємо, чи є дані
            var count = await _context.UserProfiles.CountDocumentsAsync(FilterDefinition<UserProfile>.Empty);
            if (count > 0) return;

            _logger.LogInformation("Seeding UserProfiles...");

            var users = new List<UserProfile>
            {
                new UserProfile(Guid.Parse("11111111-1111-1111-1111-111111111111"), "AlexReviewer"),
                new UserProfile(Guid.Parse("22222222-2222-2222-2222-222222222222"), "MariaCritic"),
                new UserProfile(Guid.Parse("33333333-3333-3333-3333-333333333333"), "TechGuru")
            };

            // Використання BulkWrite для ефективності
            var writes = users.Select(u => new InsertOneModel<UserProfile>(u)).ToList();
            await _context.UserProfiles.BulkWriteAsync(writes);
        }

        private async Task SeedReviewsAsync()
        {
            var count = await _context.Reviews.CountDocumentsAsync(FilterDefinition<Review>.Empty);
            if (count > 0) return;

            _logger.LogInformation("Seeding Reviews...");

            var author1 = new AuthorSnapshot(Guid.Parse("11111111-1111-1111-1111-111111111111"), "AlexReviewer");
            var productId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // iPhone X з каталогу

            var reviews = new List<Review>
            {
                new Review(productId, author1, new Rating(5), "Great phone!"),
                new Review(productId, author1, new Rating(4), "Good, but battery life could be better.")
            };

            var writes = reviews.Select(r => new InsertOneModel<Review>(r)).ToList();
            await _context.Reviews.BulkWriteAsync(writes);
        }
    }
}