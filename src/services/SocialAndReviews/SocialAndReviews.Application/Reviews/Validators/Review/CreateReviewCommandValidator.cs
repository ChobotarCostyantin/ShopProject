using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SocialAndReviews.Application.Reviews.DTOs.Requests.Review;
using SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Create;

namespace SocialAndReviews.Application.Reviews.Validators.Review
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.Request.ProductId)
                .NotEmpty().WithMessage("ProductId is required")
                .MustAsync(CheckIfProductExists)
                .WithMessage("Product with provided id does not exist");

            RuleFor(x => x.Request.AuthorId)
                .NotEmpty().WithMessage("AuthorId is required");

            RuleFor(x => x.Request.Rating)
                .NotEmpty().WithMessage("Rating is required")
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.Request.Text)
                .NotEmpty().WithMessage("Text is required")
                .MaximumLength(1000).WithMessage("Text must be less than 1000 characters");
        }

        private Task<bool> CheckIfProductExists(Guid productId, CancellationToken cancellationToken)
        {
            // TODO: Microservice communication to check if product with provided id exists
            return Task.FromResult(true);
        }
    }
}