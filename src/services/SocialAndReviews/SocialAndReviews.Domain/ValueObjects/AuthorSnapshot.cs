using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialAndReviews.Domain.Common;
using SocialAndReviews.Domain.Exceptions;

namespace SocialAndReviews.Domain.ValueObjects
{
    public class AuthorSnapshot : ValueObject
    {
        public Guid UserId { get; }
        public string Nickname { get; }

        public AuthorSnapshot(Guid userId, string nickname)
        {
            if (userId == Guid.Empty) throw new DomainException("User ID cannot be empty.");
            if (string.IsNullOrWhiteSpace(nickname)) throw new DomainException("Nickname is required.");

            UserId = userId;
            Nickname = nickname;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserId;
            yield return Nickname;
        }
    }
}