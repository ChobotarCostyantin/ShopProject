using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Requests.Comment;
using SocialAndReviews.Application.Reviews.DTOs.Responces;

namespace SocialAndReviews.Application.Reviews.UseCases.Comment.Commands.Create
{
    public record CreateCommentCommand(
        Guid ReviewId, 
        CreateCommentRequest Request
        ) : ICommand<Result<CommentDto>>;
}