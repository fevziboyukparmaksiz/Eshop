using Catalog.Data;
using Shared.CQRS;

namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid Id) 
    : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand,DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync(request.Id,cancellationToken);

        if (product is null)
        {
            throw new Exception($"Product not found : {request.Id}");
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}
