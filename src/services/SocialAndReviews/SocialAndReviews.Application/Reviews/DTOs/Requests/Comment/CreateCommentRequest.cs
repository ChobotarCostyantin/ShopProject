using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Application.Reviews.DTOs.Requests.Comment
{
    public record CreateCommentRequest(
        Guid AuthorId,
        string Text
    );
}