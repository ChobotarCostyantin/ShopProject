using Catalog.BLL.DTOs.Products.Responces;
using Orders.BLL.Features.Orders.DTOs.Responces;
using SocialAndReviews.Application.Reviews.DTOs.Responces;

namespace Aggregator.DTOs;

public record FullProductDataDto(
    ProductDto Product,
    IEnumerable<ReviewDto> Reviews,
    double AverageRating,
    IEnumerable<OrderDto> Orders
);