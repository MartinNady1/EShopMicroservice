

using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public class BasketCheckoutEventHandler(ISender sender , ILogger<BasketCheckoutEventHandler> logger) : IConsumer<BasketCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            logger.LogInformation("Integration Event: BasketCheckoutEvent consumed");
            var command = MapToCreateOrderCommand(context.Message);
            await sender.Send(command);
            
        }

        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
        {
            var addressDto  = new AddressDto(message.FirstName , message.LastName , message.AddressLine ,message.EmailAddress , message.Country, message.State , message.ZipCode);
            var paymentDto = new PaymentDto(message.CardName , message.CardNumber , message.Expiration , message.CVV , message.PaymentMethod);
            var orderId = Guid.NewGuid();
            var orderDto = new OrderDto(orderId , message.CostumerId,message.UserName , addressDto ,addressDto , paymentDto , Ordering.Domain.Enums.OrderStatus.Pending
                , [new OrderItemDto(orderId , Guid.NewGuid() ,2 , 500 ) , new OrderItemDto(orderId , Guid.NewGuid() ,1 , 500 )]
                );
            return new CreateOrderCommand(orderDto);
        }
    }
}
