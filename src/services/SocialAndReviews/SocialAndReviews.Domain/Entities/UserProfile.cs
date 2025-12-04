using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using SocialAndReviews.Domain.Common;
using SocialAndReviews.Domain.Exceptions;

namespace SocialAndReviews.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class UserProfile : BaseEntity
    {
        public string Nickname { get; private set; }
        public int ReputationScore { get; private set; }

        public UserProfile(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname)) throw new DomainException("Nickname is required.");

            Nickname = nickname;
            ReputationScore = 0;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateNickname(string newNickname)
        {
            if (string.IsNullOrWhiteSpace(newNickname)) throw new DomainException("Nickname cannot be empty.");
            Nickname = newNickname;
            UpdateTimestamp();
        }

        public void UpdateReputationScore(int newReputationScore)
        {
            if (newReputationScore < 0) throw new DomainException("Reputation score cannot be negative.");
            ReputationScore = newReputationScore;
            UpdateTimestamp();
        }
    }
}