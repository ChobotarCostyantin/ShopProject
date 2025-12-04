using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Application.Reviews.DTOs.Responces
{
    public record ReviewDto(
        Guid Id,
        Guid ProductId,
        AuthorDto Author,
        int Rating,
        string Text,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        CommentDto[] Comments
    );
}