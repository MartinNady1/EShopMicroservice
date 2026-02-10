using Mapster;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);
    public record storeBasketResponse(string UserName);
    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/basket", async (StoreBasketRequest request, ISender Sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                var result = await Sender.Send(command);
                var response = result.Adapt<storeBasketResponse>();
                return Results.Created($"/api/basket/{response.UserName}", response);

            });
        }
    }
}
