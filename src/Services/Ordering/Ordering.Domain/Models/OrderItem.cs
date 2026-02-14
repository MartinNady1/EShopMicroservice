using Ordering.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Models
{
    public class OrderItem : Entity<OrderItemId>
    {
        public OrderId OrderId { get; private set; }
        public ProductId ProductId { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        internal OrderItem(OrderId orderId, ProductId productId, decimal price, int quantity) {
            Id = OrderItemId.Of(Guid.NewGuid());
            OrderId = orderId;
            ProductId = productId;
            Price = price;
            Quantity = quantity;
        }
    }
}
