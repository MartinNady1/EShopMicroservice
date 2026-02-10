using BuildingBlocks.CQRS;
using Catalog.API.Models;
using JasperFx.CodeGeneration.Model;
using Marten;
using ResultPattern;

namespace Catalog.API.Products.GetProdcutById
{
    public record GetProductByIdQuery(Guid ProductId) : IQuery<Result<GetProductByIdResult>>;
    public record GetProductByIdResult(Product Product);
    internal class GetProdcutByIdQueryHandler(IDocumentSession _session )
        : IQueryHandler<GetProductByIdQuery, Result<GetProductByIdResult>>
    {
        public async Task<Result<GetProductByIdResult>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product =  await _session.LoadAsync<Product>(query.ProductId , cancellationToken);
            if (product is null)
            {
               
                return Result<GetProductByIdResult>.Failure(Error.NotFound($"Product with id {query.ProductId} not found"));
            }
            return Result<GetProductByIdResult>.Success(new GetProductByIdResult(product));

        }
    }
}
