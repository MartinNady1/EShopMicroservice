using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;
using ResultPattern;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<Result<DeleteProductResult>>;
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
        }
    }
    internal class DeleteProductHandler(IDocumentSession _session)
        : ICommandHandler<DeleteProductCommand, Result<DeleteProductResult>>
    {
        public async Task<Result<DeleteProductResult>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            
            _session.Delete<Product>(request.Id);
            await _session.SaveChangesAsync(cancellationToken);
           
            return Result<DeleteProductResult>.Success(new DeleteProductResult(true));
        }
    }
}
