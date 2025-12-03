using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace SocialAndReviews.Infrastructure.Database
{
    public static class DatabaseShared
    {
        private static readonly Lazy<InsertOneOptions> LazyInsertOneOptions = new Lazy<InsertOneOptions>(() => new InsertOneOptions());

        public static InsertOneOptions EmptyInsertOneOptions()
        {
            return LazyInsertOneOptions.Value;
        }
    }
}