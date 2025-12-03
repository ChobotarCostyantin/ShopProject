using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Infrastructure.Options;

namespace SocialAndReviews.Infrastructure.Database
{
    public class SocialAndReviewsDbContext
    {
        private readonly IMongoDatabase _database;
        public IMongoClient Client { get; }
        public IClientSessionHandle? Session { get; set; }
        private const string ReviewsCollectionName = "Reviews";
        private const string UserProfilesCollectionName = "UserProfiles";

        public SocialAndReviewsDbContext(IMongoClient client, IOptions<MongoOptions> options)
        {
            Client = client;
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<Review> Reviews => _database.GetCollection<Review>(ReviewsCollectionName);
        public IMongoCollection<UserProfile> UserProfiles => _database.GetCollection<UserProfile>(UserProfilesCollectionName);
    }
}
