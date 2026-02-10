using Carter;
using Catalog.API.Models;
using Catalog.API.Products.CreateProduct;
using Mapster;
using MediatR;

namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductsByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/products/category/{category}", async (string category,ISender sennder) =>
            {
                var result = await sennder.Send(new GetProductsByCategoryQuery(category));
                var response = result.Value.Adapt<GetProductsByCategoryResponse>();
                return Results.Ok(response);
                
            })
                .WithName("GetProductsByCategory")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get products by category")
                .WithDescription("Get products by category");
        }
    }
}
