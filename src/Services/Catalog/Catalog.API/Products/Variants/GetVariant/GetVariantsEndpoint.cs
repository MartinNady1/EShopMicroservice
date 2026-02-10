using Carter;
using Catalog.API.Models;
using Catalog.API.Products.GetProdcutById;
using MediatR;
using System.Net;

namespace Catalog.API.Products.Variants.GetVariant
{
    public record GetVariantByProductIdResponse(IEnumerable<ProductVariant> Variants);
    public class GetVariantsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/products/{productId:guid}/variants", async (Guid productId, ISender sender) =>
            {
                var result = await sender.Send(new GetVariantsQuery(productId));
                return result.Map(
                    onSuccess: res => Results.Ok(new GetVariantByProductIdResponse(res.Variants)),
                    onFailure: error => error.Code switch {
                        HttpStatusCode.NotFound => Results.NotFound(error.Message),
                        _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
                    }
                );
            })
                .WithName("GetVariantsByProductId")
                .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithSummary("Get variants by product id")
                .WithDescription("Get variants by product id");

        }
    }
}
