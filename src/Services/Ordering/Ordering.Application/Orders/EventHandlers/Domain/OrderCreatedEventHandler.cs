using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;
using Ordering.Domain.Events;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> _logger, IPublishEndpoint _publishEndpoint, IFeatureManager featureManager)
        : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order is successfully created. OrderId : {OrderId}", domainEvent.GetType().Name);

            if (await featureManager.IsEnabledAsync("OrderingFullfilment"))
            {
                var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();
                await _publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
            }

           
        }
    }
}
