using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrdersByName;

namespace Ordering.API.Endpoints
{
    //public record GetOrdersByNameRequest(string Name);
    public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);
    public class GetOrdersByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
           app.MapGet("api/orders/{name}", async (string name, ISender sender) =>
            {
                var query = new GetOrdersByNameQuery(name);
                var result = await sender.Send(query);
                var response = result.Adapt<GetOrdersByNameResult>();
                return Results.Ok(response);
            });
        }
    }
}
