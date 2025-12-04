using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Application.UserProfiles.DTOs.Responces;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.UserProfiles.UseCases.Commands.Create
{
    public sealed class CreateUserProfileCommandHandler : ICommandHandler<CreateUserProfileCommand, Result<UserProfileDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateUserProfileCommand> _createUserProfileCommandValidator;

        public CreateUserProfileCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IValidator<CreateUserProfileCommand> createUserProfileCommandValidator
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createUserProfileCommandValidator = createUserProfileCommandValidator;
        }

        public async Task<Result<UserProfileDto>> Handle(CreateUserProfileCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _createUserProfileCommandValidator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return Result<UserProfileDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var userProfile = new UserProfile(command.Request.Nickname);

            await _unitOfWork.UserProfileRepository.AddAsync(userProfile, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<UserProfileDto>.Created($"/api/userprofiles/{userProfile.Id}", _mapper.Map<UserProfileDto>(userProfile));
        }
    }
}