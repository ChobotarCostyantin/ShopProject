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
            CreateMap<Review, ReviewDto>()
                .ForCtorParam(nameof(ReviewDto.AuthorId), opt =>
                    opt.MapFrom(src => src.Author.UserId))
                .ForCtorParam(nameof(ReviewDto.AuthorName), opt =>
                    opt.MapFrom(src => src.Author.Nickname))
                .ForCtorParam(nameof(ReviewDto.Rating), opt =>
                    opt.MapFrom(src => src.Rating.Value))
                .ForCtorParam(nameof(ReviewDto.Comments), opt =>
                    opt.MapFrom(src => src.Comments));

            CreateMap<Comment, CommentDto>()
                .ForCtorParam(nameof(CommentDto.CommentId), opt =>
                    opt.MapFrom(src => src.CommentId))
                .ForCtorParam(nameof(CommentDto.AuthorName), opt =>
                    opt.MapFrom(src => src.Author.Nickname))
                .ForCtorParam(nameof(CommentDto.AuthorId), opt =>
                    opt.MapFrom(src => src.Author.UserId))
                .ForCtorParam(nameof(CommentDto.CreatedAt), opt =>
                    opt.MapFrom(src => src.CreatedAt));
        }
    }
}