using Ordering.Domain.Abstraction;
using Ordering.Domain.Enums;
using Ordering.Domain.Events;

namespace Ordering.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
        public CustomerId CustomerId { get; private set; }
        public OrderName OrderName { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus OrderStatus { get; private set; } = OrderStatus.Pending;
        public decimal TotalPrice
        {
            get => _orderItems.Sum(x => x.Price * x.Quantity);
            private set { }

        }
        public static Order Create(OrderId orderId, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {

            var order = new Order
            {
                Id = orderId,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                OrderStatus = OrderStatus.Pending
            };
            order.AddDomainEvent(new OrderCreatedEvent(order));
            return order;
        }
        public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment , OrderStatus orderStatus)
        {
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            OrderStatus = orderStatus;
            AddDomainEvent(new OrderUpdatedEvent(this));
        }
        public void UpdateOrderStatus(CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            CustomerId = customerId;
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            AddDomainEvent(new OrderUpdatedEvent(this));
        }
        public void AddOrderItem(ProductId productId, decimal price, int quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var orderItem = new OrderItem(Id,productId, price, quantity);
            _orderItems.Add(orderItem);
          
        }
            public void RemoveOrderItem(ProductId productId)
            {
                var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
                if (orderItem is not  null)
                {
                    _orderItems.Remove(orderItem);
                }
        }

    }
}
