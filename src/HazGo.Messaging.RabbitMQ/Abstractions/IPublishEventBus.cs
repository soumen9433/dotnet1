using HazGo.Messaging.Abstraction;
using System.Collections.Generic;

namespace HazGo.Messaging.RabbitMQ.Abstractions
{
    public interface IPublishEventBus
    {
        void Publish(IntegrationEvent integrationEvent,
            string exchangeName, 
            string exchangeType, 
            string queueName, 
            string routingKey, int deliveryMode,
            Dictionary<string, object> queueProperties,
            Dictionary<string, object> exchangeProperties);
    }
}
