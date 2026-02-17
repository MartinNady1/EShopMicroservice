using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public record GetOrdersByNameQuery(string Name) : IQuery<GetOrdersByNameResult>;
    public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);
    public class GetOrdersByNameQueryValidator : AbstractValidator<GetOrdersByNameQuery>
    {
        public GetOrdersByNameQueryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        }
    }

}
