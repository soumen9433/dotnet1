namespace HazGo.Infrastructure
{
    using MediatR;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using HazGo.BuildingBlocks.Core.Domain;
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ApplicationDbContext _dbContext;
        public DomainEventDispatcher(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DispatchEventsAsync()
        {
            var domainEntities = _dbContext.ChangeTracker
               .Entries<EntityBase<int>>()
               .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
        }
        
    }
}
