using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameHandler(IApplicationDbContext _dbcontext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery request, CancellationToken cancellationToken)
        {
            var orders = await _dbcontext
                .Orders.Include(o => o.OrderItems)
                .AsNoTracking().Where(o => o.OrderName.Value.Contains(request.Name))
                .OrderBy(o => o.OrderName)
                .ToListAsync(cancellationToken);
          
            return new GetOrdersByNameResult(orders.ToOrderDtoList());
        }
      
    }
}
