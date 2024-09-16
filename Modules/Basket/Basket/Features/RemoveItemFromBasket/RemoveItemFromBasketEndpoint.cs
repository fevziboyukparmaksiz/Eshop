using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Basket.Features.RemoveItemFromBasket;
//public record RemoveItemFromBasketRequest(string UserName, Guid ProductId);
public record RemoveItemFromBasketResponse(Guid ShoppingCartId);

public class RemoveItemFromBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{username}/items/{productId}",
                async ([FromRoute] string username,
                    [FromRoute] Guid productId,
                    ISender sender) =>
                {
                    var command = new RemoveItemFromBasketCommand(username, productId);
                    var result = await sender.Send(command);
                    var response = result.Adapt<RemoveItemFromBasketResponse>();
                    return Results.Ok(response);
                })
            .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Remove Item From Basket")
            .WithDescription("Remove Item From Basket")
            .RequireAuthorization();
    }
}