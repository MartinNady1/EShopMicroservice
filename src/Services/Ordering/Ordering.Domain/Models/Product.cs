using Ordering.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Models
{
    public class Product : Entity<ProductId>
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; } = default!;
            public static Product Create(ProductId productId, string name, decimal price)
            {
                ArgumentException.ThrowIfNullOrEmpty(name);
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
                return new Product { Id = productId, Name = name, Price = price };
        }

    }
}
