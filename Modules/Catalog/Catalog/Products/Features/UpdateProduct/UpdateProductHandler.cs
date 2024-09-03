using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Exceptions;
using Catalog.Products.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using Shared.Exceptions;

namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public class UpdateProductHandler
    (CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand,UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == command.Product.Id, cancellationToken: cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Product.Id);
        }

        UpdateProductWithNewValues(product, command.Product);

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }


    private void UpdateProductWithNewValues(Product product,ProductDto productDto)
    {
         product.Update(
             productDto.Name,
             productDto.Category,
             productDto.Description,
             productDto.ImageFile,
             productDto.Price);
    }
}
