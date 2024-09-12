using Catalog.Contracts.Products.Dtos;
using Catalog.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.CQRS;
using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest) : IQuery<GetProductsResult>;

public record GetProductsResult(PaginationResult<ProductDto> Products);
public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery,GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);

        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageSize*pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<List<ProductDto>>();

        return new GetProductsResult(new PaginationResult<ProductDto>(
            pageIndex,
            pageSize,
            totalCount,
            productDtos
            ));
    }
}
