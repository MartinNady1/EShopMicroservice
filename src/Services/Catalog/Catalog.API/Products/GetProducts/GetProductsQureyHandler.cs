using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 20) : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product> Products);
    internal class GetProductsQureyHandler(IDocumentSession _session ,ILogger<GetProductsQureyHandler> _logger )
        : IQueryHandler<GetProductsQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetProductsQureyHandler.Handle called with {@Query}" ,query);
            var products = await _session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 20, cancellationToken);
            return new GetProductResult(products);

        }
    }
}
