using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Services;
using SocialAndReviews.Infrastructure.Serializers;

namespace SocialAndReviews.Infrastructure.Database
{
    public static class DbSetup
    {
        public static async Task SetupDatabase(this WebApplication app)
        {
            ConfigureConventions();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var indexService = services.GetRequiredService<IIndexCreationService>();
            await indexService.CreateIndexesAsync();

            var seeder = services.GetRequiredService<IDataSeeder>();
            await seeder.SeedAsync();
        }

        private static void ConfigureConventions()
        {
            // ... (Ваш попередній код конфігурації серіалізаторів та ClassMaps) ...
            // Залиште код з попередньої відповіді тут без змін
            try
            {
                BsonSerializer.RegisterSerializer(new AuthorSnapshotSerializer());
                BsonSerializer.RegisterSerializer(new RatingSerializer());
                BsonSerializer.RegisterSerializer(new CommentSerializer());
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Utc));
            }
            catch (BsonSerializationException) { }

            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreExtraElementsConvention(true),
                new CamelCaseElementNameConvention()
            };
            ConventionRegistry.Register("SocialAndReviewsConventions", pack, t => true);

            RegisterClassMaps();
        }

        private static void RegisterClassMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Review)))
            {
                BsonClassMap.RegisterClassMap<Review>(cm =>
                {
                    cm.AutoMap();
                    cm.MapField("_comments").SetElementName("Comments");
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(UserProfile)))
            {
                BsonClassMap.RegisterClassMap<UserProfile>(cm =>
                {
                    cm.AutoMap();
                    cm.MapCreator(u => new UserProfile(u.Nickname));
                });
            }
        }
    }
}