using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using UUIDNext;

namespace Catalog.API.Products.Variants.CreadVariant
{
    public class CreateVariantHandler
    {
        public record CreateVariantCommand(Guid ProductId, string Color, string Size, decimal Price, int Stock) : ICommand<CreateVariantResult>
        {

        }
        public record CreateVariantResult(Guid VariantId)
        {
        }
        internal class CreateVariantHandlerHandler(IDocumentSession _session) : ICommandHandler<CreateVariantCommand, CreateVariantResult>
        {
            public async Task<CreateVariantResult> Handle(CreateVariantCommand command, CancellationToken cancellationToken)
            {
                var product = await _session.LoadAsync<Product>(command.ProductId, cancellationToken);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }
                var variant = new ProductVariant
                {
                    Id = Uuid.NewDatabaseFriendly(Database.PostgreSql),
                    ProductId = command.ProductId,
                    Color = command.Color,
                    Size = command.Size,
                    Price = command.Price,
                    Stock = command.Stock
                };
                product.Variants.Add(variant);

                _session.Update(product);
                await _session.SaveChangesAsync();

                return new CreateVariantResult(variant.Id);
            }
        }
    }
}
