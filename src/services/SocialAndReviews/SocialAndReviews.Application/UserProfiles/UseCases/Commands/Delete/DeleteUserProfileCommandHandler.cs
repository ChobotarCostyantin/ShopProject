using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.UserProfiles.UseCases.Commands.Delete
{
    public sealed class DeleteUserProfileCommandHandler : ICommandHandler<DeleteUserProfileCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DeleteUserProfileCommandHandler(
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteUserProfileCommand command, CancellationToken cancellationToken)
        {
            var userProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(command.Id, cancellationToken);
            if (userProfile is null)
            {
                return Result<bool>.NotFound(key: command.Id, entityName: nameof(Domain.Entities.UserProfile));
            }

            await _unitOfWork.UserProfileRepository.RemoveAsync(userProfile);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<bool>.NoContent();
        }
    }
}