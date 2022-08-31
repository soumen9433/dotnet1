namespace HazGo.BuildingBlocks.Core.Domain
{
    using System;

    public interface IDomainEvent// : INotification
    {
        bool IsPublished { get; set; }

        DateTime OccuredOn { get; }
    }
}
