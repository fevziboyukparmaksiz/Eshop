using Basket.Dtos;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Basket.Features.CreateBasket;

public record CreateBasketRequest(ShoppingCartDto ShoppingCart);
public record CreateBasketResponse(Guid Id);
public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (CreateBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateBasketCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateBasketResponse>();
                return Results.Created($"/basket/{response.Id}", response);
            })
            .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Basket")
            .WithDescription("Create Basket");
    }
}
