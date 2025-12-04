using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Requests.Review;
using SocialAndReviews.Application.Reviews.DTOs.Responces;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Create
{
    public record CreateReviewCommand(
        CreateReviewRequest Request
        ) : ICommand<Result<ReviewDto>>;
}