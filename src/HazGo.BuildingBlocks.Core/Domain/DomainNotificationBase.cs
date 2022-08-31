namespace HazGo.BuildingBlocks.Core.Domain
{
    using System;
    using System.Text.Json.Serialization;
    using HazGo.BuildingBlocks.Core.Domain;

    public class DomainNotificationBase<T> : IDomainEventNotification<T>
        where T : IDomainEvent
    {
        public DomainNotificationBase(T domainEvent)
        {
            this.Id = Guid.NewGuid();
            this.DomainEvent = domainEvent;
        }

        [JsonIgnore]
        public T DomainEvent { get; }

        public Guid Id { get; }
    }
}
