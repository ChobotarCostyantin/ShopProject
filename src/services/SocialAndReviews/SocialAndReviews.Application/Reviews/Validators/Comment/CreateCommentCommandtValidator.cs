using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SocialAndReviews.Application.Reviews.DTOs.Requests.Comment;
using SocialAndReviews.Application.Reviews.UseCases.Comment.Commands.Create;

namespace SocialAndReviews.Application.Reviews.Validators.Comment
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.Request.AuthorId)
                .NotEmpty().WithMessage("AuthorId is required");
                
            RuleFor(x => x.Request.Text)
                .NotEmpty().WithMessage("Text is required")
                .MaximumLength(1000).WithMessage("Text must be less than 1000 characters");
        }
    }
}