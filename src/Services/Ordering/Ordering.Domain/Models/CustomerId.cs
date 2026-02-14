using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Models
{
    public record CustomerId
    {
        public Guid Value { get; }
        private CustomerId(Guid value) => Value = value;
        public static CustomerId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                // here we can add our own custom exception type or use the built-in one, but for simplicity, we will use the built-in one. (domain layer exception)
                throw new ArgumentException("CustomerId cannot be empty.", nameof(value));
            }
            return new CustomerId(value);
        }
    }
   
}
