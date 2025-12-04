using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using SocialAndReviews.Domain.ValueObjects;

namespace SocialAndReviews.Infrastructure.Serializers
{
    public class RatingSerializer : SerializerBase<Rating>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Rating value)
        {
            context.Writer.WriteInt32(value.Value);
        }

        public override Rating Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadInt32();
            return new Rating(value);
        }
    }
}