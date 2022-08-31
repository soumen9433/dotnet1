using HazGo.Messaging.Abstraction;

namespace HazGo.Messaging.RabbitMQ.Abstractions
{
    /// <summary>
    ///  Event subscription interface
    /// </summary>
    public interface IConsumeEventBus
    {
        /// <summary>
        /// Subscribe event here
        /// </summary>
        /// <typeparam name="T">Event Object </typeparam>
        /// <typeparam name="TH">Event handler</typeparam>
        /// <param name="exchangeName">exchange name</param>
        /// <param name="queueName">queue name</param>
        /// <param name="routingKey">route name</param>
        void Subscribe<T, TH>(string exchangeName, string queueName, string routingKey)
            where T : IntegrationEvent
            where TH : IEventHandler<T>;
    }
}
