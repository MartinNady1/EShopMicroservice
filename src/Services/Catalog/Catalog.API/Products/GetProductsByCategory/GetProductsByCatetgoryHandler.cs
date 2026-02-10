using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using ResultPattern;

namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryQuery(string CategoryName) : IQuery<Result<GetProductByCategoryResult>>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    internal class GetProductsByCatetgoryHandler(IDocumentSession _session)
        : IQueryHandler<GetProductsByCategoryQuery, Result<GetProductByCategoryResult>>
    {
        public async Task<Result<GetProductByCategoryResult>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
           
            var products = await _session.Query<Product>().Where(x => x.Category.Contains(request.CategoryName)).ToListAsync(cancellationToken);
            if(!products.Any())
            {
                return Result<GetProductByCategoryResult>.Failure(Error.NotFound($"No products found for category {request.CategoryName}"));
            }
           
            return Result<GetProductByCategoryResult>.Success(new GetProductByCategoryResult(products));
        }
    }
}
