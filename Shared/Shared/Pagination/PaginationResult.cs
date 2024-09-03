namespace Shared.Pagination;

public record PaginationResult<TEntity>(
    int PageIndex,
    int PageSize,
    long Count,
    IEnumerable<TEntity> Data) 
    where TEntity : class;

  
