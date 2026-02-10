using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;
using ResultPattern;
using UUIDNext;

namespace Catalog.API.Products.CreateProduct
{


    public record CreateProductCommand(string Name, List<string> Category, List<ProductVariant> Variants, string Brand, string Description, string ImageFile, decimal Price)
        : ICommand<Result<CreateProductResult>>
    {

    }
    public record CreateProductResult(Guid ProductId)
    {

    }
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("At least one category is required");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required");
        }
    }
    internal class CreateProductCommandHandler(IDocumentSession _session ) : ICommandHandler<CreateProductCommand, Result<CreateProductResult>>
    {
        public async Task<Result<CreateProductResult>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
           
            var Product = new Product
            {
                Id = Uuid.NewDatabaseFriendly(Database.PostgreSql),
                Name = command.Name,
                Category = command.Category,
                Brand = command.Brand,
                Variants = command.Variants,
                Description = command.Description,
                ImageFile = command.ImageFile,
               
                Price = command.Price,
            };
            foreach (var variant in Product.Variants)
            {
                variant.ProductId = Product.Id;
                variant.Id = Uuid.NewDatabaseFriendly(Database.PostgreSql);
            }
            _session.Store(Product);
            await _session.SaveChangesAsync(cancellationToken);

            return Result<CreateProductResult>.Success(new CreateProductResult(Product.Id));

        }
    }
}
