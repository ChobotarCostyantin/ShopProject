using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Application.UserProfiles.DTOs.Requests;
using SocialAndReviews.Application.UserProfiles.DTOs.Responces;

namespace SocialAndReviews.Application.UserProfiles.UseCases.Commands.Create
{
    public record CreateUserProfileCommand(CreateUserProfileRequest Request) : ICommand<Result<UserProfileDto>>;
}