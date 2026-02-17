namespace Ordering.Domain.Models
{
    public readonly record struct OrderItemId
    {
        public Guid Value { get; }

        public OrderItemId(Guid value)
        {
            Value = value;
        }
        public static OrderItemId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new ArgumentException("OrderItemId cannot be empty.", nameof(value));
            }
            return new OrderItemId(value);
        }
    }
}
