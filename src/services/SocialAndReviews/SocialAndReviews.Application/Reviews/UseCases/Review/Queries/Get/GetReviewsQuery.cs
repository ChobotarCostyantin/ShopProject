using Shared.CQRS.Queries;
using Shared.DTOs;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Responces;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Queries.Get
{
    public record GetReviewsQuery(PaginationRequest Request) : IQuery<Result<PaginationResult<ReviewDto>>>;
}