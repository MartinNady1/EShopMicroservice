
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> _logger) : INotificationHandler<OrderUpdatedEvent>
    {
      

        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
        _logger.LogInformation("Order is successfully updated. OrderId : {OrderId}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
