namespace HazGo.BuildingBlocks.Core.Domain
{
    using System.Collections.Generic;

    public interface IHaveDomainEvent
    {
        public IList<DomainEventBase> DomainEvents { get; protected set; }
    }
}
