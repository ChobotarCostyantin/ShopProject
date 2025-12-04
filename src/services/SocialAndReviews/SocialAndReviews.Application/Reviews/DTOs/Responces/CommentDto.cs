using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Application.Reviews.DTOs.Responces
{
    public record CommentDto(
        Guid CommentId,
        string Text,
        Guid AuthorId,
        string AuthorName,
        DateTime CreatedAt
    );
}