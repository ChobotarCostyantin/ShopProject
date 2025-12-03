using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using SocialAndReviews.Infrastructure.Serializers;

namespace SocialAndReviews.Infrastructure.Database
{
    public static class DbSetup
    {
        public static async Task SetupDatabase(this WebApplication app)
        {
            ConfigureConventions();
            await SeedCollections(app);
        }

        private static async Task SeedCollections(WebApplication app)
        {
            
        }

        private static void ConfigureConventions()
        {
            BsonSerializer.RegisterSerializer(new AuthorSnapshotSerializer());
            BsonSerializer.RegisterSerializer(new RatingSerializer());
            BsonSerializer.RegisterSerializer(new CommentSerializer());
            BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));
        }
    }
}