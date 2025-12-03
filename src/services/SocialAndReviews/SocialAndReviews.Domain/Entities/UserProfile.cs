using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using SocialAndReviews.Domain.Common;
using SocialAndReviews.Domain.Exceptions;

namespace SocialAndReviews.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string Nickname { get; private set; }
        public int ReputationScore { get; private set; }

        public UserProfile(Guid userId, string nickname)
        {
            if (userId == Guid.Empty) throw new DomainException("User ID is required.");
            if (string.IsNullOrWhiteSpace(nickname)) throw new DomainException("Nickname is required.");

            UserId = userId;
            Nickname = nickname;
            ReputationScore = 0;
        }

        public void UpdateNickname(string newNickname)
        {
            if (string.IsNullOrWhiteSpace(newNickname)) throw new DomainException("Nickname cannot be empty.");
            Nickname = newNickname;
            UpdateTimestamp();
        }

        public void IncreaseReputation(int points)
        {
            if (points <= 0) throw new DomainException("Points must be positive.");

            ReputationScore += points;
            UpdateTimestamp();
        }
    }
}