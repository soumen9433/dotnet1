using HazGo.Messaging.Abstraction;
using System.Threading.Tasks;

namespace EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
        where TIntegrationEvent: IntegrationEvent
    {
        Task HandleAsync(TIntegrationEvent integrationEvent);
    }

    public interface IIntegrationEventHandler
    {
    }
}
