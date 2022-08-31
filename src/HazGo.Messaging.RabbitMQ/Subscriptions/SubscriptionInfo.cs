using System;

namespace HazGo.Messaging.RabbitMQ
{
    public partial class EventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        public  class SubscriptionInfo
        {
            public bool IsDynamic { get; }
            public Type HandlerType { get; }

            public SubscriptionInfo(bool isDynamic, Type handlerType)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
            }

            public static SubscriptionInfo Dynamic(Type handlerType)
            {
                return new SubscriptionInfo(true, handlerType);
            }

            public static SubscriptionInfo Typed(Type handlerType)
            {
                return new SubscriptionInfo(false, handlerType);
            }
        }
    }

}
