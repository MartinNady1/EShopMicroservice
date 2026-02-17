using BuildingBlocks.Pagintaion;
using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints
{
  
    public record GetOrdersResponse(IEnumerable<OrderDto> Orders);
    public class GetOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
             app.MapGet("api/orders", async ([AsParameters] PaginationRequest query,ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(query));
                var response = result.Adapt<GetOrdersResponse>();
                return Results.Ok(response);
            });
        }
    }
}
