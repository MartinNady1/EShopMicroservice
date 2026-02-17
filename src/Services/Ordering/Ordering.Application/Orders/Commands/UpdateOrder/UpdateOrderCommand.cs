using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{

    public record UpdateOrderCommand(OrderDto order) : ICommand<UpdateOrderResult>;
    public record UpdateOrderResult(bool Success);
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.order.Id).NotEmpty().WithMessage("Order Id cannot be empty");
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
