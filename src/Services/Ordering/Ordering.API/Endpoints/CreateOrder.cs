using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints
{
    public record CreateOrdersRequest(OrderDto Order);
    public record CreateOrdersResponse(Guid Id);
    public class CreateOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/orders", async (OrderDto Order, ISender sender) =>
            {
                var command = Order.Adapt<CreateOrderCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<GetOrdersResponse>();
                return Results.Created($"/api/orders/{response.Orders}", response);
            });
        }
    }
}
