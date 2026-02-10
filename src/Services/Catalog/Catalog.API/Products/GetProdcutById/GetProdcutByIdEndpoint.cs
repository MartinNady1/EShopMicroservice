using Carter;
using Catalog.API.Models;
using MediatR;
using System.Net;

namespace Catalog.API.Products.GetProdcutById
{
    public record GetProductByIdResponse(Product Product);
    public class GetProdcutByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/products/{productId:guid}", async (Guid productId, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(productId));
                return ResultToHttpMapper.Map(result);
            })
                .WithName("GetProductById")
                .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithSummary("Get product by id")
                .WithDescription("Get product by id");
            ;
        }
    }
    
    
}
