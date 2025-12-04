using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTOs;
using SocialAndReviews.Application.Reviews.DTOs.Responces;

namespace Aggregator.Services
{
    public class SocialAndReviewsClient
    {
        private readonly HttpClient _client;

        public SocialAndReviewsClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<PaginationResult<ReviewDto>?> GetReviewsByProductIdAsync(Guid productId, CancellationToken ct = default)
        {
            return await _client.GetFromJsonAsync<PaginationResult<ReviewDto>>($"api/reviews/{productId}/reviews", ct);
        }
    }
}