

using Ordering.Application.Dtos;
using Ordering.Domain.Models;

namespace Ordering.Application.Extensions
{
    public static class OrderExtensions
    {
        public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
        {


            return orders.Select(order => new OrderDto
             (
                  order.Id.Value,
                  order.CustomerId.Value,
                  order.OrderName.Value,
                  new AddressDto
                 (
                      order.ShippingAddress.FirstName,
                      order.ShippingAddress.LastName,
                      order.ShippingAddress.State,
                     order.ShippingAddress.ZipCode,
                     order.ShippingAddress.Country,
                     order.ShippingAddress.EmailAddress,
                     order.ShippingAddress.AddressLine
                 ),
                  new AddressDto
                 (
                     order.BillingAddress.FirstName,
                     order.BillingAddress.LastName,
                     order.BillingAddress.State,
                     order.BillingAddress.ZipCode,
                     order.BillingAddress.Country,
                     order.BillingAddress.EmailAddress,
                     order.BillingAddress.AddressLine
                 ),
                 new PaymentDto
                 (

                     order.Payment.CardHolderName,
                     order.Payment.CardNumber,
                     order.Payment.ExpirationDate,
                     order.Payment.CVV,
                     order.Payment.PaymentMethod
                 ),
                  order.OrderStatus,
                  order.OrderItems.Select(oi => new OrderItemDto
                 (
                      oi.Id.Value,
                      oi.ProductId.Value,
                      oi.Price,
                      oi.Quantity
                 )).ToList()
             ));

        }

        public static OrderDto ToOrderDto(this Order order)
        {
            return DtoFromOrder(order);
        }

        private static OrderDto DtoFromOrder(Order order)
        {
            return new OrderDto(
                order.Id.Value,
                  order.CustomerId.Value,
                  order.OrderName.Value,
                  new AddressDto
                 (
                      order.ShippingAddress.FirstName,
                      order.ShippingAddress.LastName,
                      order.ShippingAddress.State,
                     order.ShippingAddress.ZipCode,
                     order.ShippingAddress.Country,
                     order.ShippingAddress.EmailAddress,
                     order.ShippingAddress.AddressLine
                 ),
                  new AddressDto
                 (
                     order.BillingAddress.FirstName,
                     order.BillingAddress.LastName,
                     order.BillingAddress.State,
                     order.BillingAddress.ZipCode,
                     order.BillingAddress.Country,
                     order.BillingAddress.EmailAddress,
                     order.BillingAddress.AddressLine
                 ),
                 new PaymentDto
                 (

                     order.Payment.CardHolderName,
                     order.Payment.CardNumber,
                     order.Payment.ExpirationDate,
                     order.Payment.CVV,
                     order.Payment.PaymentMethod
                 ),
                  order.OrderStatus,
                  order.OrderItems.Select(oi => new OrderItemDto
                 (
                      oi.Id.Value,
                      oi.ProductId.Value,
                      oi.Price,
                      oi.Quantity
                 )).ToList()
                );
        }
    }
}
