using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Shared.CQRS.Queries;
using Shared.DTOs;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Responces;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Queries.Get
{
    public sealed class GetReviewsQueryHandler : IQueryHandler<GetReviewsQuery, Result<PaginationResult<ReviewDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetReviewsQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PaginationResult<ReviewDto>>> Handle(GetReviewsQuery query, CancellationToken cancellationToken)
        {
            var paginationResult = await _unitOfWork.ReviewRepository.GetReviewsAsync(query.Request.PageNumber, query.Request.PageSize, cancellationToken);

            var paginatedDtos = paginationResult.ToPaginatedDtos(
                (review) => _mapper.Map<ReviewDto>(review)
            );

            return Result<PaginationResult<ReviewDto>>.Ok(paginatedDtos);
        }
    }
}