using Catalog.Data;
using Catalog.Products.Exceptions;
using FluentValidation;
using Shared.CQRS;

namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid Id) 
    : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
    }
}

public class DeleteProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<DeleteProductCommand,DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync(command.Id,cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteProductResult(true);
    }
}
