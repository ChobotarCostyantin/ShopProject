using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Services;
using SocialAndReviews.Infrastructure.Database;

namespace SocialAndReviews.Infrastructure.Services
{
    public class IndexCreationService : IIndexCreationService
    {
        private readonly SocialAndReviewsDbContext _context;
        private readonly ILogger<IndexCreationService> _logger;

        public IndexCreationService(SocialAndReviewsDbContext context, ILogger<IndexCreationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateIndexesAsync()
        {
            _logger.LogInformation("Starting MongoDB index creation...");

            // 1. Reviews Indexes
            var reviewBuilder = Builders<Review>.IndexKeys;
            var reviewIndexes = new List<CreateIndexModel<Review>>
            {
                new CreateIndexModel<Review>(
                    reviewBuilder.Ascending(r => r.ProductId),
                    new CreateIndexOptions { Name = "ProductId_Asc" }
                ),
                // Compound index for sorting
                new CreateIndexModel<Review>(
                    reviewBuilder.Ascending(r => r.ProductId).Descending(r => r.CreatedAt),
                    new CreateIndexOptions { Name = "ProductId_Asc_CreatedAt_Desc" }
                ),
                // Text index for search
                new CreateIndexModel<Review>(
                    reviewBuilder.Text(r => r.Text),
                    new CreateIndexOptions { Name = "Text_Content" }
                )
            };
            await _context.Reviews.Indexes.CreateManyAsync(reviewIndexes);

            // 2. UserProfiles Indexes
            var profileBuilder = Builders<UserProfile>.IndexKeys;
            var profileIndexes = new List<CreateIndexModel<UserProfile>>
            {
                // Unique indexes
                new CreateIndexModel<UserProfile>(
                    profileBuilder.Ascending(u => u.UserId),
                    new CreateIndexOptions { Unique = true, Name = "UserId_Unique" }
                ),
                new CreateIndexModel<UserProfile>(
                    profileBuilder.Ascending(u => u.Nickname),
                    new CreateIndexOptions { Unique = true, Name = "Nickname_Unique" }
                )
            };
            await _context.UserProfiles.Indexes.CreateManyAsync(profileIndexes);

            _logger.LogInformation("MongoDB index creation completed.");
        }
    }
}