using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.CQRS.Commands;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Requests.Comment;
using SocialAndReviews.Application.Reviews.DTOs.Responces;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.Interfaces.Repositories;
using SocialAndReviews.Domain.ValueObjects;

namespace SocialAndReviews.Application.Reviews.UseCases.Comment.Commands.Create
{
    public sealed class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, Result<CommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IValidator<CreateCommentCommand> _createCommentCommandValidator;

        public CreateCommentCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateCommentCommand> createCommentCommandValidator
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createCommentCommandValidator = createCommentCommandValidator;
        }

        public async Task<Result<CommentDto>> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
        {
            var validationResult = _createCommentCommandValidator.Validate(command);
            if (!validationResult.IsValid)
            {
                return Result<CommentDto>.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(command.ReviewId, cancellationToken);
            if (review is null)
            {
                return Result<CommentDto>.NotFound(key: command.ReviewId, entityName: nameof(Domain.Entities.Review));
            }

            var userProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(command.Request.AuthorId, cancellationToken);
            if (userProfile is null)
            {
                return Result<CommentDto>.NotFound(key: command.Request.AuthorId, entityName: nameof(UserProfile));
            }

            var snapshot = new AuthorSnapshot(userProfile.Id, userProfile.Nickname);

            var comment = new Domain.ValueObjects.Comment(command.Request.Text, snapshot);

            review.AddComment(comment.Text, comment.Author);

            await _unitOfWork.ReviewRepository.UpdateAsync(review);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<CommentDto>.Created($"/api/reviews/{review.Id}/comments/{comment.CommentId}", _mapper.Map<CommentDto>(comment));
        }
    }
}