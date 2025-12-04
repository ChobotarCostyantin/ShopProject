using AutoMapper;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.Reviews.UseCases.Comment.Commands.Delete
{
    public sealed class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(command.ReviewId, cancellationToken);
            if (review is null)
            {
                return Result<bool>.NotFound(key: command.ReviewId, entityName: nameof(Review));
            }

            var comment = review.Comments.FirstOrDefault(c => c.CommentId == command.CommentId);
            if (comment is null)
            {
                return Result<bool>.NotFound(key: command.CommentId, entityName: nameof(Domain.ValueObjects.Comment));
            }

            review.RemoveComment(command.CommentId);

            await _unitOfWork.ReviewRepository.UpdateAsync(review);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.NoContent();
        }
    }
}