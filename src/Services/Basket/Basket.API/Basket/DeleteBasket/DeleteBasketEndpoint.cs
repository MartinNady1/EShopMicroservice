


namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketResponse(bool IsSuccess);
    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/Basket/{Username}", async (string Username, ISender Sender) =>
            {
                var result = await Sender.Send(new DeleteBasketCommand(Username));
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            });
        }
    }
}
