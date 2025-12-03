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
    }
}