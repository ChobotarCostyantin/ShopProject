using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Application.UserProfiles.DTOs.Responces;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.UserProfiles.UseCases.Commands.Update
{
    public sealed class UpdateUserProfileCommandHandler : ICommandHandler<UpdateUserProfileCommand, Result<UserProfileDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<UpdateUserProfileCommand> _updateUserProfileCommandValidator;

        public UpdateUserProfileCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IValidator<UpdateUserProfileCommand> updateUserProfileCommandValidator
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _updateUserProfileCommandValidator = updateUserProfileCommandValidator;
        }

        public async Task<Result<UserProfileDto>> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken)
        {
            var validationResult = _updateUserProfileCommandValidator.Validate(command);
            if (!validationResult.IsValid)
            {
                return Result<UserProfileDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var userProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(command.Id, cancellationToken);
            if (userProfile is null)
            {
                return Result<UserProfileDto>.NotFound(key: command.Id, entityName: nameof(Domain.Entities.UserProfile));
            }

            userProfile.UpdateNickname(command.Request.Nickname);
            userProfile.UpdateReputationScore(command.Request.ReputationScore);

            await _unitOfWork.UserProfileRepository.UpdateAsync(userProfile);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<UserProfileDto>.Ok(_mapper.Map<UserProfileDto>(userProfile));
        }
    }
}