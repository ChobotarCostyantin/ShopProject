using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Queries;
using Shared.ErrorHandling;
using SocialAndReviews.Application.UserProfiles.DTOs.Responces;

namespace SocialAndReviews.Application.UserProfiles.UseCases.Queries.GetById
{
    public record GetUserProfileByIdQuery(Guid Id) : IQuery<Result<UserProfileDto>>;
}