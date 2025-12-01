using Shared.DTOs;

namespace Catalog.BLL.DTOs.Common
{
    public record GetRequest(string? SortBy = "", bool SortDescending = false) : PaginationRequest;
}