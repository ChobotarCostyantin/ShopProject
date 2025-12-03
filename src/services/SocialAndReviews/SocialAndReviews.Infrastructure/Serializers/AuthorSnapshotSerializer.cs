using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using SocialAndReviews.Domain.ValueObjects;

namespace SocialAndReviews.Infrastructure.Serializers
{
    public class AuthorSnapshotSerializer : SerializerBase<AuthorSnapshot>
    {
        public override AuthorSnapshot Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();

            Guid userId = Guid.Empty;
            string nickname = string.Empty;

            while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var name = context.Reader.ReadName();

                switch (name)
                {
                    case "UserId":
                        userId = BsonSerializer.LookupSerializer<Guid>().Deserialize(context, args);
                        break;
                    case "Nickname":
                        nickname = context.Reader.ReadString();
                        break;
                    default:
                        // Пропускаємо невідомі поля для зворотної сумісності
                        context.Reader.SkipValue();
                        break;
                }
            }

            context.Reader.ReadEndDocument();

            return new AuthorSnapshot(userId, nickname);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, AuthorSnapshot value)
        {
            context.Writer.WriteStartDocument();
            BsonSerializer.LookupSerializer<Guid>().Serialize(context, args, value.UserId);
            context.Writer.WriteString(value.Nickname);
            context.Writer.WriteEndDocument();
        }
    }
}