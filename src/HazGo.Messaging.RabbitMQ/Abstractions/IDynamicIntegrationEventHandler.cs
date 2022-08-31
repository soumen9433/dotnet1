using System.Threading.Tasks;

namespace HazGo.Messaging.RabbitMQ.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task HandleAsync(dynamic eventData);

    }
}
