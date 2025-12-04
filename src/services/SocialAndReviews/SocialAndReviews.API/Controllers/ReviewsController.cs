using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SocialAndReviews.Application.Reviews.DTOs.Requests.Comment;
using SocialAndReviews.Application.Reviews.DTOs.Requests.Review;
using SocialAndReviews.Application.Reviews.DTOs.Responces;
using SocialAndReviews.Application.Reviews.UseCases.Comment.Commands.Create;
using SocialAndReviews.Application.Reviews.UseCases.Comment.Commands.Delete;
using SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Create;
using SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Delete;
using SocialAndReviews.Application.Reviews.UseCases.Review.Commands.Update;
using SocialAndReviews.Application.Reviews.UseCases.Review.Queries.Get;
using SocialAndReviews.Application.Reviews.UseCases.Review.Queries.GetById;
using SocialAndReviews.Application.Reviews.UseCases.Review.Queries.GetByProductId;

namespace SocialAndReviews.API.Controllers
{
    [Route("api/[controller]")]
    public class ReviewsController : BaseApiController
    {
        private readonly ISender _sender;

        public ReviewsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(CreateReviewRequest request, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new CreateReviewCommand(request), cancellationToken)).ToApiResponse();
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new GetReviewByIdQuery(id), cancellationToken)).ToApiResponse();
        }

        [HttpGet("{productId:guid}/reviews")]
        [ProducesResponseType(typeof(PaginationResult<ReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByProductIdAsync(Guid productId, [FromQuery] PaginationRequest request, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new GetReviewsByProductIdQuery(productId, request), cancellationToken)).ToApiResponse();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResult<ReviewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationRequest request, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new GetReviewsQuery(request), cancellationToken)).ToApiResponse();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(Guid id, UpdateReviewRequest request, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new UpdateReviewCommand(id, request), cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new DeleteReviewCommand(id), cancellationToken)).ToApiResponse();
        }

        [HttpPost("{reviewId:guid}/comments")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCommentAsync(Guid reviewId, CreateCommentRequest request, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new CreateCommentCommand(reviewId, request), cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{reviewId:guid}/comments/{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCommentAsync(Guid reviewId, Guid commentId, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new DeleteCommentCommand(reviewId, commentId), cancellationToken)).ToApiResponse();
        }
    }
}