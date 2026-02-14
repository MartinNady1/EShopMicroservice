using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Models
{
    public record Payment
    {
        public string CardNumber { get; init; } = default!;
        public string CardHolderName { get; init; } = default!;
        public string ExpirationDate { get; init; } = default!;
        public string CVV { get; init; } = default!;
        public int PaymentMethod { get; init; } = default!;
        protected Payment() { }
        private Payment(string cardNumber, string cardHolderName, string expirationDate, string cvv, int paymentMethod)
        {
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            ExpirationDate = expirationDate;
            CVV = cvv;
            PaymentMethod = paymentMethod;
        }
        public static Payment Create(string cardNumber, string cardHolderName, string expirationDate, string cvv, int paymentMethod)
        {
            ArgumentException.ThrowIfNullOrEmpty(cardNumber);
            ArgumentException.ThrowIfNullOrEmpty(cardHolderName);
            ArgumentException.ThrowIfNullOrEmpty(expirationDate);
            ArgumentException.ThrowIfNullOrEmpty(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);
            return new Payment(cardNumber, cardHolderName, expirationDate, cvv, paymentMethod);
        }

    }
}
