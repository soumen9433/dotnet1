namespace HazGo.BuildingBlocks.Core.Domain
{
    using System;

    public abstract class DomainEventBase : IDomainEvent
    {
        protected DomainEventBase()
        {
            OccuredOn = DateTime.UtcNow;
        }

        public bool IsPublished { get; set; }

        public DateTime OccuredOn { get; protected set; }
    }
}
