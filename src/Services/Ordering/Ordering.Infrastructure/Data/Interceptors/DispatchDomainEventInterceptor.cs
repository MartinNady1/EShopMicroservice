using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class DispatchDomainEventInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        public async Task DispatchDomainEvents(DbContext? context)
        {
            if (context == null) return;
                        var domainEntities = context.ChangeTracker
                .Entries<IAggregate>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }

        }
    }
}
