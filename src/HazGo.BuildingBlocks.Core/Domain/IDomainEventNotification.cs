namespace HazGo.BuildingBlocks.Core.Domain
{
    using System;

    public interface IDomainEventNotification<out TEventType>// : IDomainEventNotification
    {
        TEventType DomainEvent { get; }
    }
}