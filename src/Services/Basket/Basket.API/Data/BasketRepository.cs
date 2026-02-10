
using Marten;

namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession _session) : IBasketRepository

    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
            _session.Delete<ShoppingCart>(userName);
            await _session.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
        {
            var basket = await _session.LoadAsync<ShoppingCart>(userName, cancellationToken);
            return basket is null ? throw new Exception("Basket not found") : basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken)
        {
            _session.Store(basket);
            await _session.SaveChangesAsync(cancellationToken);
            return basket;
        }
    }
}
