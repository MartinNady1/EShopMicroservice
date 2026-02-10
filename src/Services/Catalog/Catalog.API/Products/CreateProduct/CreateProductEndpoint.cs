using Carter;
using Catalog.API.Models;
using Mapster;
using MediatR;
namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Category,
        List<ProductVariant> Variants, string Brand, string Description, string ImageFile, decimal Price);
    public record CreateProductResponse(Guid ProductId);
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                if (!result.IsSuccess) 
                    return ResultToHttpMapper.Map(result);
                var response = result.Value.Adapt<CreateProductResponse>();
                return Results.Created($"/api/products/{response.ProductId}", response);
            })
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Creates a new product")
                .WithDescription("Creates a new product with the provided details and returns the created product's ID.");
        }
    }
}