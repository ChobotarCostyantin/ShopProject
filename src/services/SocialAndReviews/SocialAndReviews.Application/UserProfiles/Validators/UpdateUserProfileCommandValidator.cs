using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MongoDB.Driver;
using SocialAndReviews.Application.UserProfiles.UseCases.Commands.Update;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.UserProfiles.Validators
{
    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserProfileCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Request.Nickname)
                .NotEmpty().WithMessage("Nickname is required")
                .MustAsync(async (command, newNickname, cancellationToken) => 
                {
                    return await IsNicknameAvailableForUpdateAsync(
                        newNickname, 
                        command.Id,
                        cancellationToken);
                })
                .WithMessage("Nickname is already in use");

            RuleFor(x => x.Request.ReputationScore)
                .NotEmpty().WithMessage("Reputation score is required")
                .GreaterThan(0).WithMessage("Reputation score must be greater than 0");
        }

        private async Task<bool> IsNicknameAvailableForUpdateAsync(
            string nickname, 
            Guid currentUserId, 
            CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserProfileRepository.IsNicknameAvailableForUpdateAsync(
                nickname, 
                currentUserId, 
                cancellationToken);
        }
    }
}