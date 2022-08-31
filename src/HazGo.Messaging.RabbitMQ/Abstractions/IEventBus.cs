using HazGo.Messaging.Abstraction;
using System.Collections.Generic;

namespace HazGo.Messaging.RabbitMQ.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent integrationEvent,
            string exchangeName, 
            string exchangeType, 
            string queueName, 
            string routingKey, int deliveryMode,
            Dictionary<string, object> queueProperties);

        void Subscribe<T, TH>(string exchangeName, string queueName,string routingKey)
            where T : IntegrationEvent
            where TH : IEventHandler<T>;
    }
}
