using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Responces;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Repositories;
using SocialAndReviews.Domain.ValueObjects;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Create
{
    public sealed class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand, Result<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateReviewCommand> _createReviewCommandValidator;

        public CreateReviewCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            IValidator<CreateReviewCommand> createReviewCommandValidator
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createReviewCommandValidator = createReviewCommandValidator;
        }

        public async Task<Result<ReviewDto>> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _createReviewCommandValidator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return Result<ReviewDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var userProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(command.Request.AuthorId, cancellationToken);
            if (userProfile is null)
            {
                return Result<ReviewDto>.NotFound(key: command.Request.AuthorId, entityName: nameof(UserProfile));
            }

            var snapshot = new AuthorSnapshot(userProfile.Id, userProfile.Nickname);
            var rating = new Rating(command.Request.Rating);

            var review = new Domain.Entities.Review(
                command.Request.ProductId,
                snapshot,
                rating,
                command.Request.Text
            );

            await _unitOfWork.ReviewRepository.AddAsync(review, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<ReviewDto>.Created($"/api/reviews/{review.Id}", _mapper.Map<ReviewDto>(review));
        }
    }
}