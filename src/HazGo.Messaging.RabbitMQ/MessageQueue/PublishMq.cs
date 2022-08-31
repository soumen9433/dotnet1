using HazGo.Messaging.RabbitMQ.Abstractions;
using HazGo.Messaging.RabbitMQ.Connection;
using HazGo.Messaging.Abstraction;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Text;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System;
using System.Diagnostics;

namespace EventBus.MessageQueue
{
    public class PublishMq : IPublishEventBus
    {
        readonly IMqConnection _connection;
        readonly ILogger<PublishMq> _logger;
        private IModel _consumerChannel;
        private readonly int _retryCount;

        public PublishMq(IMqConnection connection, ILogger<PublishMq> logger, int retryCount)
        {
            _connection = connection;
            _logger = logger;
            _retryCount = retryCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event">Event class reference</param>
        /// <param name="exchangeName">Set exchange name</param>
        /// <param name="exchangeType">Set exchange type as direct,topic,fanout,header  </param>
        /// <param name="queueName">Set the queue name</param>
        /// <param name="routingKey">Set up valid route key</param>
        /// <param name="deliveryMode">Set value to 2 for persistent messages</param>
        /// <param name="queueProperties">Refer https://www.rabbitmq.com/queues.html </param>
        /// <param name="exchangeProperty"></param>
        public void Publish(IntegrationEvent integrationEvent,
            string exchangeName, string exchangeType,
            string queueName, string routingKey, int deliveryMode,
            Dictionary<string, object> queueProperties, Dictionary<string, object> exchangeProperties)
        {
            _logger.LogTrace("Creating channel to publish event: {EventId} ({RouteKey})", integrationEvent.Id, routingKey);

            using (var channel = _connection.CreateModel())
            {

                var policy = RetryPolicy.Handle<BrokerUnreachableException>()
               .Or<SocketException>()
               .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
               {
                   _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", integrationEvent.Id, $"{time.TotalSeconds:n1}", ex.Message);
               });

                _logger.LogTrace("Declaring exchange to publish event: {EventId}", integrationEvent.Id);

                channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, arguments: exchangeProperties, durable: true);

                channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: queueProperties);


                channel.QueueBind(queue: queueName,
                   exchange: exchangeName,
                   routingKey: routingKey
                   );

                integrationEvent.PublishDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var message = JsonConvert.SerializeObject(integrationEvent);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.DeliveryMode = (byte)deliveryMode;
                    properties.Timestamp = new AmqpTimestamp(Convert.ToInt64((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds));

                    _logger.LogTrace("Publishing event to Queue: {EventId} with Timestamp: {Timestamp}", integrationEvent.Id, properties.Timestamp);

                    channel.BasicPublish(
                          exchange: exchangeName,
                          routingKey: routingKey,
                          mandatory: true,
                          basicProperties: properties,
                          body: body
                          );

                    channel.ConfirmSelect();
                });
            }
        }

        private IModel CreateConsumerChannel(string queueName,
            string exchangeName,
            string exchangeType,
            Dictionary<string, object> queueProperty,
            Dictionary<string, object> exchangeProperty)
        {

            _logger.LogTrace("Creating consumer channel");

            var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchangeName,
                                    type: exchangeType,
                                    arguments: exchangeProperty);


            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: queueProperty);


            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel(queueName, exchangeName, exchangeType, queueProperty, exchangeProperty);
            };
            return channel;
        }
    }
}
