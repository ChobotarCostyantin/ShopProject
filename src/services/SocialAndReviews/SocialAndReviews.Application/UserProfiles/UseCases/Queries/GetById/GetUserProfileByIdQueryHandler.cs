using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Shared.CQRS.Queries;
using Shared.ErrorHandling;
using SocialAndReviews.Application.UserProfiles.DTOs.Responces;
using SocialAndReviews.Domain.Interfaces.Repositories;

namespace SocialAndReviews.Application.UserProfiles.UseCases.Queries.GetById
{
    public sealed class GetUserProfileByIdQueryHandler : IQueryHandler<GetUserProfileByIdQuery, Result<UserProfileDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public GetUserProfileByIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserProfileDto>> Handle(GetUserProfileByIdQuery query, CancellationToken cancellationToken)
        {
            var userProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(query.Id, cancellationToken);

            return userProfile is null
                ? Result<UserProfileDto>.NotFound(key: query.Id, entityName: nameof(Domain.Entities.UserProfile))
                : Result<UserProfileDto>.Ok(_mapper.Map<UserProfileDto>(userProfile));
        }
    }
}