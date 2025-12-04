using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SocialAndReviews.Domain.Interfaces.Repositories;
using SocialAndReviews.Domain.Interfaces.Services;
using SocialAndReviews.Infrastructure.Database;
using SocialAndReviews.Infrastructure.Options;
using SocialAndReviews.Infrastructure.Repositories;
using SocialAndReviews.Infrastructure.Services;

namespace SocialAndReviews.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
        {
            // 1. Налаштування конфігурації (MongoOptions)
            services.AddOptions<MongoOptions>()
                .BindConfiguration(MongoOptions.ConfigurationKey)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // 2. Реєстрація MongoClient (Singleton, бо він thread-safe і має connection pooling)
            // services.AddSingleton<IMongoClient>(sp =>
            // {
            //     var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
                
            //     var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);
            //     settings.MinConnectionPoolSize = options.MinPoolSize;
            //     settings.MaxConnectionPoolSize = options.MaxPoolSize;
            //     settings.ConnectTimeout = TimeSpan.FromSeconds(options.ConnectionTimeoutSeconds);

            //     return new MongoClient(settings);
            // });

            services.AddScoped<SocialAndReviewsDbContext>();

            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IIndexCreationService, IndexCreationService>();
            services.AddScoped<IDataSeeder, SocialAndReviewsSeeder>();

            services.AddHealthChecks()
                .AddMongoDb(
                    sp => sp.GetRequiredService<IMongoClient>(),
                    name: "mongodb",
                    timeout: TimeSpan.FromSeconds(3)
                );

            return services;
        }
    }
}