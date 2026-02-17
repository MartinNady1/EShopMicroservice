using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler(IApplicationDbContext _dbcontext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.Id);
            var order = await _dbcontext.Orders.FindAsync([orderId], cancellationToken);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            _dbcontext.Orders.Remove(order);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return new DeleteOrderResult(true);
        }

    }
}
