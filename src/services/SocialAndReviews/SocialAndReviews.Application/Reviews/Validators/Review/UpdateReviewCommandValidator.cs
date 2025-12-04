using FluentValidation;
using SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Update;

namespace SocialAndReviews.Application.Reviews.Validators.Review
{
    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.Request.Text)
                .NotEmpty().WithMessage("Text is required")
                .MaximumLength(1000).WithMessage("Text must be less than 1000 characters");

            RuleFor(x => x.Request.Rating)
                .NotEmpty().WithMessage("Rating is required")
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
        }
    }
}