using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using SocialAndReviews.Domain.Common;
using SocialAndReviews.Domain.Exceptions;
using SocialAndReviews.Domain.ValueObjects;

namespace SocialAndReviews.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Review : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public AuthorSnapshot Author { get; private set; }
        public Rating Rating { get; private set; }
        public string Text { get; private set; }
        private List<Comment> _comments = [];
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        public Review(Guid productId, AuthorSnapshot author, Rating rating, string text)
        {
            if (productId == Guid.Empty) throw new DomainException("Product ID is required.");

            ProductId = productId;
            Author = author ?? throw new DomainException("Author is required.");
            Rating = rating ?? throw new DomainException("Rating is required.");
            Text = text ?? throw new DomainException("Review text cannot be empty.");
        }

        public void UpdateText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText)) throw new DomainException("New text cannot be empty.");

            Text = newText;
            UpdateTimestamp();
        }

        public void ChangeRating(Rating newRating)
        {
            Rating = newRating ?? throw new DomainException("Rating cannot be null.");
            UpdateTimestamp();
        }

        public void AddComment(string text, AuthorSnapshot commenter)
        {
            var comment = new Comment(text, commenter);
            _comments.Add(comment);
            UpdateTimestamp();
        }

        public void RemoveComment(Guid commentId)
        {
            var comment = _comments.FirstOrDefault(c => c.CommentId == commentId)
                ?? throw new DomainException("Comment not found.");
            _comments.Remove(comment);
            UpdateTimestamp();
        }
    }
}