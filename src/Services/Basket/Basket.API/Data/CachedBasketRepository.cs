
using JasperFx.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository _repository, IDistributedCache _cache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
           
            await _repository.DeleteBasket(userName, cancellationToken);
            await _cache.RemoveAsync(userName, cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
        {
            var cachedBasket = await _cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
                JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);

                var basket = await _repository.GetBasket(userName, cancellationToken);
                await _cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
                return basket;
            
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken)
        {
            await _repository.StoreBasket(basket , cancellationToken);
            await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
           
            return basket;
        }
    }
}
