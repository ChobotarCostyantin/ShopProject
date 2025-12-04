using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Delete
{
    public sealed class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(command.Id, cancellationToken);
            if (review is null)
            {
                return Result<bool>.NotFound(key: command.Id, entityName: nameof(Domain.Entities.Review));
            }
            await _unitOfWork.ReviewRepository.RemoveAsync(review);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<bool>.NoContent();
        }
    }
}