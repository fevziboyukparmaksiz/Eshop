using Catalog.Contracts.Products.Dtos;
using Catalog.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.CQRS;

namespace Catalog.Products.Features.GetProductByCategory;

public record GetProductByCategoryQuery(string CategoryName) 
    : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<ProductDto> Products);

public class GetProductByCategoryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByCategoryQuery,GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(query.CategoryName))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        var productDtos = product.Adapt<List<ProductDto>>();

        return new GetProductByCategoryResult(productDtos);
    }
}
