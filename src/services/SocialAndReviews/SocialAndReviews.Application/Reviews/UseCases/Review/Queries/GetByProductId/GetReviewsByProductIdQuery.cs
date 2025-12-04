using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Queries;
using Shared.DTOs;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Responces;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Queries.GetByProductId
{
    public record GetReviewsByProductIdQuery(Guid ProductId, PaginationRequest Request) : IQuery<Result<PaginationResult<ReviewDto>>>;
}