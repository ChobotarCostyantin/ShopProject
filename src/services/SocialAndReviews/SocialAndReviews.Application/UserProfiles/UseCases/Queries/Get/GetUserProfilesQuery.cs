using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Queries;
using Shared.DTOs;
using Shared.ErrorHandling;
using SocialAndReviews.Application.UserProfiles.DTOs.Responces;

namespace SocialAndReviews.Application.UserProfiles.UseCases.Queries.Get
{
    public record GetUserProfilesQuery(PaginationRequest Request) : IQuery<Result<PaginationResult<UserProfileDto>>>;
}