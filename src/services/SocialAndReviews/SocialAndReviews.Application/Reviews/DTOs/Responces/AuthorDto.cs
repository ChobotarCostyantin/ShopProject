using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAndReviews.Application.Reviews.DTOs.Responces
{
    public record AuthorDto(
        Guid Id,
        string Nickname
    );
}