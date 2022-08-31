using HazGo.BuildingBlocks.Core.Domain;
using HazGo.BuildingBlocks.Persistence.EF;

namespace HazGo.Infrastructure
{
    public class UnitOfWork : UnitOfWorkBase<ApplicationDbContext>
    {
        public UnitOfWork(ApplicationDbContext dbContext, IDomainEventDispatcher domainEventDispatcher)
             : base(dbContext, domainEventDispatcher)
        {
        }
    }
}
