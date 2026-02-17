using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints
{
    public record UpdateOrderRequest(OrderDto Order);
    public record UpdateOrderResponse(bool IsSuccess);
    public class UpdateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/orders", async (UpdateOrderRequest Order, ISender sender) =>
                    {
                        var command = Order.Adapt<UpdateOrderCommand>();
                        var result = await sender.Send(command);
                        var response = result.Adapt<UpdateOrderResponse>();
                        return Results.Ok(response);
                    });
        }
    }
}
