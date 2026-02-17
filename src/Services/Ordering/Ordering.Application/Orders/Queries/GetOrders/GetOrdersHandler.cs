
using BuildingBlocks.CQRS;
using BuildingBlocks.Pagintaion;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IApplicationDbContext _dbcontext) :IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var totalCount = await _dbcontext.Orders.LongCountAsync(cancellationToken);
            var orders = await _dbcontext.Orders
                .Include(x => x.OrderItems)
                .OrderBy(o=>o.OrderName.Value)
                .Skip((query.PaginationRequest.pageIndex - 1) * query.PaginationRequest.pageSize)
                .Take(query.PaginationRequest.pageSize)
                .ToListAsync(cancellationToken);

            return new GetOrdersResult(
                new PaginatedResult<OrderDto>(
                    pageIndex: query.PaginationRequest.pageIndex,
                    pageSize: query.PaginationRequest.pageSize,
                    count: totalCount,
                    items: (List<OrderDto>)orders.ToOrderDtoList()));
           
        }
    }
}
