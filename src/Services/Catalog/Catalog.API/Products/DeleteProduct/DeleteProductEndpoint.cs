using Carter;
using MediatR;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/products/{id}", async (Guid id , ISender sender) =>
            {
                var  result = await sender.Send(new DeleteProductCommand(id));
                return ResultToHttpMapper.Map(result);
            });
        }
    }
}
