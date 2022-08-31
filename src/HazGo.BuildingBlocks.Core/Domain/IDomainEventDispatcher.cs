namespace HazGo.BuildingBlocks.Core.Domain
{
    using System.Threading.Tasks;

    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync();
    }
}
