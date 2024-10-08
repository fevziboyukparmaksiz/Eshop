using Basket.Data.Repositories;
using Shared.Contracts.CQRS;

namespace Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string Username) 
    : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketHandler(IBasketRepository basketRepository) 
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        await basketRepository.DeleteBasket(command.Username, cancellationToken);

        return new DeleteBasketResult(true);
    }
}
