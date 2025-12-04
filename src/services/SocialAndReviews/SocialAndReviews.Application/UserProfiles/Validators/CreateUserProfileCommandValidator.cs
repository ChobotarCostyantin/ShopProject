using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MongoDB.Driver;
using Shared.Extensions;
using SocialAndReviews.Application.UserProfiles.DTOs.Requests;
using SocialAndReviews.Application.UserProfiles.UseCases.Commands.Create;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.UserProfiles.Validators
{
    public class CreateUserProfileCommandValidator : AbstractValidator<CreateUserProfileCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateUserProfileCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Request.Nickname)
                .NotEmpty().WithMessage("Nickname is required")
                .MustAsync(async (nickname, cancellationToken) => 
                    await IsNicknameUniqueAsync(nickname, cancellationToken))
                .WithMessage("Nickname is already in use");
        }

        public async Task<bool> IsNicknameUniqueAsync(string nickname, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserProfileRepository.IsNicknameUniqueAsync(nickname, cancellationToken);
        }
    }
}