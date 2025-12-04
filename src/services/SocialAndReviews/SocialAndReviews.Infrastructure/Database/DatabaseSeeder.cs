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
            await SeedDatabaseAsync();
        }

        public async Task SeedDatabaseAsync()
        {
            // Ідемпотентність: перевіряємо, чи є дані
            var userCount = await _context.UserProfiles.CountDocumentsAsync(FilterDefinition<UserProfile>.Empty);
            var reviewCount = await _context.Reviews.CountDocumentsAsync(FilterDefinition<Review>.Empty);
            if (userCount > 0 && reviewCount > 0) return;

            _logger.LogInformation("Seeding user profiles...");

            var users = new List<UserProfile>
            {
                new UserProfile("AlexReviewer"),
                new UserProfile("MariaCritic"),
                new UserProfile("TechGuru")
            };

            var userWrites = users.Select(u => new InsertOneModel<UserProfile>(u)).ToList();

            _logger.LogInformation("Seeding reviews...");


            var author1 = new AuthorSnapshot(users[0].Id, users[0].Nickname);
            var author2 = new AuthorSnapshot(users[1].Id, users[1].Nickname);
            var productId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // iPhone X з каталогу

            var reviews = new List<Review>
            {
                new Review(productId, author1, new Rating(5), "Great phone!"),
                new Review(productId, author2, new Rating(4), "Good, but battery life could be better.")
            };

            var reviewWrites = reviews.Select(r => new InsertOneModel<Review>(r)).ToList();

            await Task.WhenAll(_context.UserProfiles.BulkWriteAsync(userWrites), _context.Reviews.BulkWriteAsync(reviewWrites));
        }
    }
}