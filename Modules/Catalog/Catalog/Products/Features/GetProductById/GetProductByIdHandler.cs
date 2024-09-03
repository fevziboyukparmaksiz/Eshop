using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(ProductDto Product);

public class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken: cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(query.Id);
        }

        var productDto = product.Adapt<ProductDto>();

        return new GetProductByIdResult(productDto);
    }
}
