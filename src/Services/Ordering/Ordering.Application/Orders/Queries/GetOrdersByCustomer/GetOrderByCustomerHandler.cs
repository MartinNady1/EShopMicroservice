
using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Extensions;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrderByCustomerHandler(IApplicationDbContext _dbcontext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerQueryResult>
    {
        public async Task<GetOrdersByCustomerQueryResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
        {
            var order = await _dbcontext.Orders.Include(x => x.OrderItems)
                            .AsNoTracking()
                            .Where(x => x.CustomerId == CustomerId.Of(query.CustomerId))
                            .OrderBy(o => o.OrderName.Value)
                            .ToListAsync(cancellationToken);
            return new GetOrdersByCustomerQueryResult(order.ToOrderDtoList());
        }
    }
}
