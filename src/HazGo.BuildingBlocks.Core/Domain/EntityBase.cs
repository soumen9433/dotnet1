namespace HazGo.BuildingBlocks.Core.Domain
{
    using System.Collections.Generic;

    public abstract class EntityBase : EntityBase<long>
    {
        protected EntityBase()
        {
        }

        protected EntityBase(long id)
            : base(id)
        {
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Common type and generic type")]
    public abstract class EntityBase<TPrimaryKey>
    {
        private List<IDomainEvent> _domainEvents;

        protected EntityBase()
        {
        }

        protected EntityBase(TPrimaryKey id)
        {
            Id = id;
        }

        public virtual TPrimaryKey Id { get; protected set; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            this._domainEvents.Add(domainEvent);
        }
    }
}
