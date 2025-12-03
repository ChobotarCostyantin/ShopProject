using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialAndReviews.Domain.Common
{
    public abstract class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        protected BaseEntity()
        {
            Id = Guid.CreateVersion7();
            CreatedAt = DateTime.UtcNow;
        }

        protected void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}