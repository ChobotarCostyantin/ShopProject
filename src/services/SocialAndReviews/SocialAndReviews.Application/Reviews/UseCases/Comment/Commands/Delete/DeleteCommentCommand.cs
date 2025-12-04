using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;

namespace SocialAndReviews.Application.Reviews.UseCases.Comment.Commands.Delete
{
    public record DeleteCommentCommand(Guid CommentId, Guid ReviewId) : ICommand<Result<bool>>;
}