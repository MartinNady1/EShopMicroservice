
using Basket.API.Data;

namespace Basket.API.Basket.DeleteBasket
{
    public  record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess );
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    { 
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    public class DeleteBasketCommandHandler(IBasketRepository _basket) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            await  _basket.DeleteBasket(request.UserName , cancellationToken);
            return  new DeleteBasketResult(true);
        }
    }
}
