using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Queries;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Responces;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Queries.GetById
{
    public record GetReviewByIdQuery(Guid Id) : IQuery<Result<ReviewDto>>;
}