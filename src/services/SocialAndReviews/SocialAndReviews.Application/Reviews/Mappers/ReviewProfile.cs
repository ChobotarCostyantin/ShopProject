using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SocialAndReviews.Application.Reviews.DTOs.Responces;
using SocialAndReviews.Domain.Entities;
using SocialAndReviews.Domain.ValueObjects;

namespace SocialAndReviews.Application.Reviews.Mappers
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            // Мапінг для автора
            CreateMap<AuthorSnapshot, AuthorDto>()
                .ConstructUsing(src => new AuthorDto(src.UserId, src.Nickname));

            // Мапінг для Review
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForCtorParam(nameof(ReviewDto.Rating), opt => opt.MapFrom(src => src.Rating.Value))
                .ForCtorParam(nameof(ReviewDto.Comments), opt => opt.MapFrom(src => src.Comments));

            // Мапінг для Comment
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForCtorParam(nameof(CommentDto.CommentId), opt => opt.MapFrom(src => src.CommentId))
                .ForCtorParam(nameof(CommentDto.CreatedAt), opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}