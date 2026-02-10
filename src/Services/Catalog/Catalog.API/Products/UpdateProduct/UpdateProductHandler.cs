using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;
using ResultPattern;
using System.Windows.Input;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id,string Name, List<string> Category,
        List<ProductVariant> Variants, string Brand, string Description, string ImageFile, decimal Price): ICommand<Result<UpdateProductResult>>;
    public record UpdateProductResult(bool IsSuccess );
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required");
                RuleFor(x => x.Category).NotEmpty().WithMessage("At least one category is required");
                RuleFor(x => x.Variants).NotEmpty().WithMessage("At least one variant is required");
                RuleFor(x => x.Brand).NotEmpty().WithMessage("Brand is required");
                RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
                RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required");
                RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
                RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
              
        }
    }
    internal class UpdateProductHandler(IDocumentSession _session) : ICommandHandler<UpdateProductCommand, Result<UpdateProductResult>>
    {
        public async Task<Result<UpdateProductResult>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _session.LoadAsync<Product>(command.Id , cancellationToken);
            if (product is null)
            {
                return Result<UpdateProductResult>.Failure(Error.NotFound("Product not found"));
            }
            product.Name = command.Name;
            product.Category = command.Category;
            product.Variants = command.Variants;
            product.Brand = command.Brand;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;
            _session.Update(product);
            await _session.SaveChangesAsync(cancellationToken);
            return Result<UpdateProductResult>.Success(new UpdateProductResult(true));

        }
    }
}
