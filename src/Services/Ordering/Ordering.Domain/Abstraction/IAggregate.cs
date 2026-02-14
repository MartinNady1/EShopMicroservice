using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Domain.Abstraction
{
    public interface IAggregate<T> : IAggregate , IEntity<T> { 



    }
    public interface IAggregate : IEntity
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        IDomainEvent[] ClearDomainEvents();
    }
}
