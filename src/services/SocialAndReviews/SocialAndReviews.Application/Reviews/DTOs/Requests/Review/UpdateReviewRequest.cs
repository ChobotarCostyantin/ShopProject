using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Application.Reviews.DTOs.Requests.Review
{
    public record UpdateReviewRequest(
        string Text,
        int Rating
    );
}