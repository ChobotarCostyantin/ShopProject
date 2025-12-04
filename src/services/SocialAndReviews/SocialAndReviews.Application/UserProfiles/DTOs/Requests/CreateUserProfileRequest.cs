using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Application.UserProfiles.DTOs.Requests
{
    public record CreateUserProfileRequest(
        string Nickname
    );
}