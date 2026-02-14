using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Abstraction
{
    public interface IDomainEvent : INotification
    {
        Guid EventId => new Guid();
        public DateTime OccurredOn => DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}
