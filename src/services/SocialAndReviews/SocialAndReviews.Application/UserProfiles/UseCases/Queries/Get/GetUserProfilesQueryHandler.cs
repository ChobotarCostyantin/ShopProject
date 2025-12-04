using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Shared.CQRS.Queries;
using Shared.DTOs;
using Shared.ErrorHandling;
using SocialAndReviews.Application.UserProfiles.DTOs.Responces;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.UserProfiles.UseCases.Queries.Get
{
    public sealed class GetUserProfilesQueryHandler : IQueryHandler<GetUserProfilesQuery, Result<PaginationResult<UserProfileDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserProfilesQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PaginationResult<UserProfileDto>>> Handle(GetUserProfilesQuery query, CancellationToken cancellationToken)
        {
            var paginationResult = await _unitOfWork.UserProfileRepository.GetUserProfilesAsync(query.Request.PageNumber, query.Request.PageSize, cancellationToken);

            var paginatedDtos = paginationResult.ToPaginatedDtos(
                (userProfile) => _mapper.Map<UserProfileDto>(userProfile)
            );

            return Result<PaginationResult<UserProfileDto>>.Ok(paginatedDtos);
        }
    }
}