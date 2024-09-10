using Basket.Dtos;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Basket.Features.AddItemToBasket;

public record AddItemToBasketRequest(string Username, ShoppingCartItemDto ShoppingCartItem);
public record AddItemToBasketResponse(Guid ShoppingCartId);

public class AddItemToBasketEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{username}/items", 
                async ([FromRoute] string username, 
                    [FromBody] AddItemToBasketRequest request, 
                    ISender sender) =>
                {
                    var command = new AddItemToBasketCommand(username, request.ShoppingCartItem);
                    var result = await sender.Send(command);
                    var response = result.Adapt<AddItemToBasketResponse>();
                    return Results.Created($"/basket/{response.ShoppingCartId}", response);
                })
            .Produces<AddItemToBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Item Into Basket")
            .WithDescription("Add Item Into Basket");
    }
}
