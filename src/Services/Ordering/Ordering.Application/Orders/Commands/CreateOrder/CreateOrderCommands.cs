using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(OrderDto order) : ICommand<CreateOrderResult>;
    public record CreateOrderResult(Guid OrderId);
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.order).NotNull().WithMessage("Order cannot be null");
            RuleFor(x => x.order.CustomerId).NotEmpty().WithMessage("CustomerId cannot be empty");
            RuleFor(x => x.order.OrderName).NotEmpty().WithMessage("OrderName cannot be empty");
            RuleFor(x => x.order.ShippingAddress).NotNull().WithMessage("ShippingAddress cannot be null");
            RuleFor(x => x.order.BillingAddress).NotNull().WithMessage("BillingAddress cannot be null");
            RuleFor(x => x.order.Payment).NotNull().WithMessage("Payment cannot be null");
            RuleFor(x => x.order.Items).NotEmpty().WithMessage("Items cannot be empty");

        }
    }
 
}