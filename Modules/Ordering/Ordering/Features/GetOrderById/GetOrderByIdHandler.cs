using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using Ordering.Dtos;
using Ordering.Exceptions;
using Shared.Contracts.CQRS;

namespace Ordering.Features.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IQuery<GetOrderByIdResult>;

public record GetOrderByIdResult(OrderDto Order);
public class GetOrderByIdHandler(OrderingDbContext dbContext) 
    : IQueryHandler<GetOrderByIdQuery,GetOrderByIdResult>
{
    public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(query.Id);
        }

        var orderDto = order.Adapt<OrderDto>();

        return new GetOrderByIdResult(orderDto);
    }
}
