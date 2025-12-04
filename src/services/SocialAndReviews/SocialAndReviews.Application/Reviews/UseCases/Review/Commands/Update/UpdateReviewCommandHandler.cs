using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Responces;
using SocialAndReviews.Domain.Interfaces.Repositories;
using SocialAndReviews.Domain.ValueObjects;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Update
{
    public sealed class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, Result<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<UpdateReviewCommand> _updateReviewCommandValidator;

        public UpdateReviewCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IValidator<UpdateReviewCommand> updateReviewCommandValidator
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _updateReviewCommandValidator = updateReviewCommandValidator;
        }

        public async Task<Result<ReviewDto>> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
        {
            var validationResult = _updateReviewCommandValidator.Validate(command);
            if (!validationResult.IsValid)
            {
                return Result<ReviewDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(command.Id, cancellationToken);
            if (review is null)
            {
                return Result<ReviewDto>.NotFound(key: command.Id, entityName: nameof(Domain.Entities.Review));
            }

            review.UpdateText(command.Request.Text);
            review.ChangeRating(new Rating(command.Request.Rating));

            await _unitOfWork.ReviewRepository.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<ReviewDto>.Ok(_mapper.Map<ReviewDto>(review));
        }
    }
}