using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using SocialAndReviews.Domain.ValueObjects;

namespace SocialAndReviews.Infrastructure.Serializers
{
    public class CommentSerializer : SerializerBase<Comment>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Comment value)
        {
            context.Writer.WriteStartDocument();

            context.Writer.WriteName("CommentId");
            context.Writer.WriteGuid(value.CommentId);

            context.Writer.WriteName("Text");
            context.Writer.WriteString(value.Text);

            context.Writer.WriteName("Author");
            BsonSerializer.Serialize(context.Writer, value.Author);

            context.Writer.WriteName("CreatedAt");
            context.Writer.WriteDateTime(value.CreatedAt.Ticks);
            
            context.Writer.WriteEndDocument();
        }

        public override Comment Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            context.Reader.ReadStartDocument();

            Guid commentId = Guid.Empty;
            string text = string.Empty;
            AuthorSnapshot? author = null;
            DateTime createdAt = DateTime.MinValue;

            while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var name = context.Reader.ReadName();

                switch (name)
                {
                    case "CommentId":
                        commentId = context.Reader.ReadGuid();
                        break;
                    case "Text":
                        text = context.Reader.ReadString();
                        break;
                    case "Author":
                        author = BsonSerializer.Deserialize<AuthorSnapshot>(context.Reader);
                        break;
                    case "CreatedAt":
                        var bsonDateTime = context.Reader.ReadDateTime();
                        createdAt = new DateTime(bsonDateTime, DateTimeKind.Utc);
                        break;
                    default:
                        context.Reader.SkipValue();
                        break;
                }
            }

            context.Reader.ReadEndDocument();

            if (author == null)
                throw new BsonSerializationException("AuthorSnapshot is missing in Comment document.");

            return new Comment(commentId, text, author, createdAt);
        }
    }
}