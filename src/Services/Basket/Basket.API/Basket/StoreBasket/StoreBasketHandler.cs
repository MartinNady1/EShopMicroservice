using Basket.API.Data;
using Discount.Grpc.Protos;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) :ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName cannot be empty");
          
           
        }
    }
    public  class StoreBasketCommandHandler(IBasketRepository _basket , DiscountProtoService.DiscountProtoServiceClient _discountProto) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async  Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
                foreach (var item in command.Cart.Items)
            {
                var coupon = await _discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
            
            await _basket.StoreBasket(command.Cart , cancellationToken);
            return new StoreBasketResult(command.Cart.UserName);
            
        }
    }
}
