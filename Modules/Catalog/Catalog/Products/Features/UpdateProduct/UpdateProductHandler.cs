using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;

namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand,UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Product.Id, cancellationToken: cancellationToken);

        if (product is null)
        {
            throw new Exception($"Product not found : {request.Product.Id}");
        }

        UpdateProductWithNewValues(product, request.Product);

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
