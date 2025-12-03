using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Infrastructure.Options
{
    public class MongoOptions
    {
        public const string ConfigurationKey = "MongoOptions";

        [Required]
        public string ConnectionString { get; init; } = null!;

        [Required]
        public string DatabaseName { get; init; } = null!;

        // Налаштування для Connection Pooling та Timeouts
        public int MinPoolSize { get; init; } = 5;
        public int MaxPoolSize { get; init; } = 100;
        public int ConnectionTimeoutSeconds { get; init; } = 30;
    }
}