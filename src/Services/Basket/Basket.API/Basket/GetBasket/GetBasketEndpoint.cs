


using Mapster;

namespace Basket.API.Basket.GetBasket
{
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/basket/{userName}", async (string Username, ISender Sender) =>
            {
                var result = await Sender.Send(new GetBasketQuery(Username));
                var response = result.Adapt<GetBasketResult>();
                return Results.Ok(response);
            });
        }
    }
}
