using BuildingBlocks.CQRS;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext _dbcontext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = CreateNewOrder(command.order);
            _dbcontext.Orders.Add(order);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return new CreateOrderResult(order.Id.Value);
        }
        private Order CreateNewOrder(OrderDto order)
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

            var newOrder = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(order.CustomerId),
                OrderName.Of(order.OrderName),
                shippingAddress,
                billingAddress,
                Payment.Of(
                    cardNumber: order.Payment.CardNumber,
                cardHolderName: order.Payment.CardName,
                expirationDate: order.Payment.Expiration,
                cvv: order.Payment.CVV, paymentMethod: order.Payment.PaymentMethod));
            foreach (var item in order.Items)
            {
                newOrder.AddOrderItem(
                    ProductId.Of(item.ProductId),
                    item.Price,
                    item.Quantity);
            }
            return newOrder;
        }
    }
}
