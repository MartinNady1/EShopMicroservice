using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
   
    public class UpdateOrderHandler(IApplicationDbContext _dbcontext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {

        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.order.Id);
            var order = await _dbcontext.Orders.FindAsync([orderId], cancellationToken: cancellationToken);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            UpdateWithNewValues(order, command.order);
            _dbcontext.Orders.Update(order);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return new UpdateOrderResult(Success: true);

        }
        private void UpdateWithNewValues(Order order, OrderDto orderDto)
        {
            var shippingAddress = Address.Of(
                 order.ShippingAddress.FirstName,
                 order.ShippingAddress.LastName,
                 order.ShippingAddress.EmailAddress,
                 order.ShippingAddress.AddressLine,
                 order.ShippingAddress.Country,
                 order.ShippingAddress.State,
                 order.ShippingAddress.State,
                 order.ShippingAddress.ZipCode);

            var billingAddress = Address.Of(
                order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.EmailAddress,
                order.BillingAddress.AddressLine,
                order.BillingAddress.Country,
                order.BillingAddress.State,
                order.BillingAddress.State,
                order.BillingAddress.ZipCode);
            var updatedPayment = Payment.Of(
                    cardNumber: order.Payment.CardNumber,
                cardHolderName: order.Payment.CardHolderName,
                expirationDate: order.Payment.ExpirationDate,
                cvv: order.Payment.CVV, paymentMethod: order.Payment.PaymentMethod);

            order.Update(
               OrderName.Of(orderDto.OrderName),
               shippingAddress,
               billingAddress,
               updatedPayment,
               orderDto.Status
               );
        }
    }
}
