using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Application.UserProfiles.DTOs.Responces
{
    public record UserProfileDto(
        Guid Id,
        string Nickname,
        int ReputationScore,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}