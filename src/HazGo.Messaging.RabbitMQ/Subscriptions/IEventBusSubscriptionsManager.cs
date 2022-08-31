using HazGo.Messaging.RabbitMQ.Abstractions;
using System;
using System.Collections.Generic;
using static HazGo.Messaging.RabbitMQ.EventBusSubscriptionsManager;
using HazGo.Messaging.Abstraction;

namespace HazGo.Messaging.RabbitMQ
{
    public interface IEventBusSubscriptionsManager
    {
        void AddSubscription<T,TH>(string routeKey) 
           where T : IntegrationEvent
           where TH : IEventHandler;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        Type GetEventTypeByName(string eventName);
    }
}
