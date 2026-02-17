using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Dtos
{
    public record OrderItemDto(Guid OrderId, Guid ProductId, decimal Price,int  Quantity);
}
