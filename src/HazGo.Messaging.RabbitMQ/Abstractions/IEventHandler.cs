using HazGo.Messaging.Abstraction;
using System.Threading.Tasks;

namespace HazGo.Messaging.RabbitMQ.Abstractions
{
    public interface IEventHandler<in TIntegrationEvent>  : IEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task<int> HandleAsync(TIntegrationEvent integrationEvent);
    }

    public interface IEventHandler
    {
    }
}
