using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using Ordering.Dtos;
using Shared.Contracts.CQRS;
using Shared.Pagination;

namespace Ordering.Features.GetOrders;

public record GetOrdersQuery(PaginationRequest PaginationRequest) : IQuery<GetOrdersResult>;
public record GetOrdersResult(PaginationResult<OrderDto> Orders);
public class GetOrdersHandler(OrderingDbContext dbContext) 
    : IQueryHandler<GetOrdersQuery,GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .OrderBy(x => x.OrderName)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var orderDtos = order.Adapt<List<OrderDto>>();

        return new GetOrdersResult(new PaginationResult<OrderDto>(
            pageIndex,
            pageSize,
            totalCount,
            orderDtos));
    }
}
