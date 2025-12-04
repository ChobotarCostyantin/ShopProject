using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SocialAndReviews.Application.UserProfiles.DTOs.Requests;
using SocialAndReviews.Application.UserProfiles.DTOs.Responces;
using SocialAndReviews.Domain.Entities;

namespace SocialAndReviews.Application.UserProfiles.Mappers
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<CreateUserProfileRequest, UserProfile>();
            CreateMap<UserProfile, UserProfileDto>();
        }
    }
}