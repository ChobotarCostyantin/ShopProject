namespace Shared.DTOs;

public record PaginationResult<TEntity>(
    TEntity[] Entities, 
    long TotalCount,
    int CurrentPage,
    decimal TotalPages,
    int PageSize)
{
    public bool HasNext => CurrentPage < TotalPages;
    public bool HasPrevious => CurrentPage > 1;

    public static PaginationResult<TEntity> Create(TEntity[] entities, long totalCount, int currentPage, int pageSize)
    {
        return new PaginationResult<TEntity>(
            entities,
            totalCount,
            currentPage,
            Math.Ceiling((decimal)totalCount / pageSize),
            pageSize);
    }
}

public static class PaginationResultExtensions
{
    public static PaginationResult<TDto> ToPaginatedDtos<TEntity, TDto>(this PaginationResult<TEntity> src, Func<TEntity, TDto> dtoFactory)
    {
        return PaginationResult<TDto>.Create(
                src.Entities.Select(dtoFactory).ToArray(),
                src.TotalCount,
                src.CurrentPage,
                src.PageSize);
    }
}