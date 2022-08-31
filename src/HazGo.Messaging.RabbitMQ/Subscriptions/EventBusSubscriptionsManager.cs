namespace HazGo.Messaging.RabbitMQ
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HazGo.Messaging.RabbitMQ.Abstractions;
    using HazGo.Messaging.Abstraction;

    /// <summary>
    ///
    /// </summary>
    public partial class EventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBusSubscriptionsManager"/> class.
        /// </summary>
        public EventBusSubscriptionsManager()
        {
            this._handlers = new Dictionary<string, List<SubscriptionInfo>>();
            this._eventTypes = new List<Type>();
        }

        /// <inheritdoc/>
        public void AddSubscription<T, TH>(string routeKey)
            where T : IntegrationEvent
            where TH : IEventHandler
        {
            var eventName = routeKey;

            this.DoAddSubscription(typeof(TH), eventName, isDynamic: false);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }

        private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
        {
            if (!this.HasSubscriptionsForEvent(eventName))
            {
                this._handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (this._handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                this._handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                this._handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventName">Name of Event</param>
        /// <returns>Key name.</returns>
        public bool HasSubscriptionsForEvent(string eventName) => this._handlers.ContainsKey(eventName);

        
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public Type GetEventTypeByName(string eventName)
        {
            return _eventTypes.SingleOrDefault(t => t.Name == eventName);
        }
    }
}
