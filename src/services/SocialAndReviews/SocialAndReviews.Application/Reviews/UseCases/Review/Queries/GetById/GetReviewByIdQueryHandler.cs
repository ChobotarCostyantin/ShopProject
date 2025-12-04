using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Shared.CQRS.Queries;
using Shared.ErrorHandling;
using SocialAndReviews.Application.Reviews.DTOs.Responces;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.Reviews.UseCases.Review.Queries.GetById
{
    public sealed class GetReviewByIdQueryHandler : IQueryHandler<GetReviewByIdQuery, Result<ReviewDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetReviewByIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ReviewDto>> Handle(GetReviewByIdQuery query, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(query.Id, cancellationToken);

            return review is null
                ? Result<ReviewDto>.NotFound(key: query.Id, entityName: nameof(Domain.Entities.Review))
                : Result<ReviewDto>.Ok(_mapper.Map<ReviewDto>(review));
        }
    }
}