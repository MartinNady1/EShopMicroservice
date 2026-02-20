
using Basket.API.Dtos;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketRequest(BasketCheckoutDto basketCheckout);
    public record CheckoutBasketResponse(bool IsSuccess);
    public class CheckoutBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/basket/checkout",async ( CheckoutBasketRequest request , ISender sender) =>
            {
                var command = request.Adapt<CheckoutBasketCommand>();
                var result = await sender.Send(command);
                var respnse = result.Adapt<CheckoutBasketResponse>();
                return Results.Ok(respnse);
            });
        }
    }
}
