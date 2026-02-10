using BuildingBlocks.CQRS;
using Carter;
using Catalog.API.Models;
using Mapster;
using MediatR;
using ResultPattern;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category,
        List<ProductVariant> Variants, string Brand, string Description, string ImageFile, decimal Price);
    public record UpdateProductRespnose(bool IsSuccess);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/products", async (  UpdateProductRequest request, ISender sender) =>
            {
               
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                if (!result.IsSuccess) 
                    return ResultToHttpMapper.Map(result);
                var response = result.Adapt<UpdateProductRespnose>();
                return ResultToHttpMapper.Map(result);

            })
                .WithName("UpdateProduct")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Update an existing product.")
                .WithDescription("Update an existing product by providing its ID and the updated details.");
        }
    }
}
