
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> _logger, IPublishEndpoint publishEndpoint) : INotificationHandler<OrderUpdatedEvent>
    {


        public Task Handle(OrderUpdatedEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order is successfully updated. OrderId : {OrderId}", domainEvent.GetType().Name);

            
            return Task.CompletedTask;
        }
    }
}
