using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Gbm.Ordering.Domain.Common;

namespace Gbm.Ordering.Infrastructure
{
    public static class MediatrExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderingContext context)
        {
            var domainEntities = context.ChangeTracker.Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
            domainEntities.ToList().ForEach(x => x.Entity.DomainEvents.Clear());
            var tasks = domainEvents.Select(async (domainEvent) => { await mediator.PublishAsync(domainEvent); });
            await Task.WhenAll(tasks);
        }
    }
}
