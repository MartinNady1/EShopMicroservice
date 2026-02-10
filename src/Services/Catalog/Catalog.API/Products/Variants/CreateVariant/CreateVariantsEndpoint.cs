using BuildingBlocks.CQRS;
using Carter;
using Mapster;
using MediatR;
using static Catalog.API.Products.Variants.CreadVariant.CreateVariantHandler;

namespace Catalog.API.Products.Variants.CreadVariant
{

    public record CreateVariantRequest(Guid ProductId, string Color, string Size, decimal Price, int Stock);
    public record CreateVariantResponse(Guid VariantId);

    public class CreateVariantsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/products/{ProductId}/variants", async (CreateVariantRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateVariantCommand> ();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateVariantResponse>();
                return Results.Created($"/api/products/{request.ProductId}/variants/{response.VariantId}", response);
            })
                .WithName("CreateVariant")
                .Produces<CreateVariantResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .WithSummary("Creates a new variant.")
                .WithDescription("Creates a new variant.");
        }
    }
}
