using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Delete
{
    public record DeleteReviewCommand(Guid Id) : ICommand<Result<bool>>;
}