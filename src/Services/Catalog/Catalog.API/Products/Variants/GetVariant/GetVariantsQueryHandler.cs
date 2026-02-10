using BuildingBlocks.CQRS;
using Catalog.API.Models;
using ImTools;
using Marten;
using ResultPattern;

namespace Catalog.API.Products.Variants.GetVariant
{
    public record GetVariantsQuery(Guid ProductId) : IQuery<Result<GetVariantsResult>>;
    public record GetVariantsResult(IEnumerable<ProductVariant> Variants);
    internal class GetVariantsQueryHandler(IDocumentSession _session)
        : IQueryHandler<GetVariantsQuery, Result<GetVariantsResult>>
    {
        public async Task<Result<GetVariantsResult>> Handle(GetVariantsQuery query, CancellationToken cancellationToken)
        {
            var product = await _session.LoadAsync<Product>(query.ProductId, cancellationToken);
            if (product is null)
            {
                return Result<GetVariantsResult>.Failure(Error.NotFound("Product not found"));
            }
            var variants = product.Variants??Enumerable.Empty<ProductVariant>();
            if (variants is null) { return Result<GetVariantsResult>.Failure(Error.NotFound("Variants not found")); }
            return Result<GetVariantsResult>.Success(new GetVariantsResult(variants));
        }
    }
}
