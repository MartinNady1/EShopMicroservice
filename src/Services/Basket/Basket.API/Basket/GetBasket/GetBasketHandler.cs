using Basket.API.Data;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery (string Username): IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart Cart);
    public class GetBasketQueryHandler(IBasketRepository _basket) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async  Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var cart = await _basket.GetBasket(request.Username , cancellationToken);
            return new GetBasketResult(cart);
        }
    }
}
