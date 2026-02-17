namespace Ordering.Domain.Models
{
    public readonly record  struct OrderName
    {
        private const int defaultLength = 50;
        public string Value { get; }
        private OrderName(string value) => Value = value;
        public static OrderName Of(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("OrderName cannot be empty or whitespace.", nameof(value));
            }
            if (value.Length > defaultLength)
            {
                throw new ArgumentException($"OrderName cannot be longer than {defaultLength} characters.", nameof(value));
            }
            return new OrderName(value);
          
        }

    }
}
