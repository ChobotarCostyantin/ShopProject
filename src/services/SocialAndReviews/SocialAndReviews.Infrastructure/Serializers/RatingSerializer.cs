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
            context.Writer.WriteStartDocument();
            context.Writer.WriteInt32(value.Value);
            context.Writer.WriteEndDocument();
        }

        public override Rating Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();
            var value = context.Reader.ReadInt32();
            context.Reader.ReadEndDocument();
            return new Rating(value);
        }
    }
}