using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public record PaginationRequest(
    [Range(1, 50, ErrorMessage = "Page size must be a positive number")] int PageSize = 10,
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be a positive number")] int PageNumber = 1);