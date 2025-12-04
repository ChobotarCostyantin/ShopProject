using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using SocialAndReviews.Domain.Common;
using SocialAndReviews.Domain.Exceptions;

namespace SocialAndReviews.Domain.ValueObjects
{
    public class Comment : ValueObject
    {
        public Guid CommentId { get; }
        public string Text { get; }
        public AuthorSnapshot Author { get; }
        public DateTime CreatedAt { get; }

        public Comment(string text, AuthorSnapshot author)
        {
            CommentId = Guid.CreateVersion7();
            Text = text ?? throw new DomainException("Comment text cannot be empty.");
            Author = author ?? throw new DomainException("Author is required.");
            CreatedAt = DateTime.UtcNow;
        }
        public Comment(Guid commentId, string text, AuthorSnapshot author, DateTime createdAt)
        {
            CommentId = commentId;
            Text = text ?? throw new DomainException("Comment text cannot be empty.");
            Author = author ?? throw new DomainException("Author is required.");
            CreatedAt = createdAt;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CommentId;
            yield return Text;
            yield return Author;
            yield return CreatedAt;
        }
    }
}