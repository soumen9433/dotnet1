namespace HazGo.Messaging.RabbitMQ.MessageQueue
{
    using HazGo.Messaging.RabbitMQ.Abstractions;
    using HazGo.Messaging.Abstraction;
    using HazGo.Messaging.RabbitMQ.Connection;
    using Autofac;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Events;

    public enum BrokerAction
    {
        ConfirmAndAcknowledge = 0,
        RejectAndDiscard = 1,
        RejectAndRequeue = 2,
        NegetiveAcknowledgedAndRequeue = 3,
        NegetiveAcknowledgedAndDiscard = 4
    }

    public class ConsumeMq : IConsumeEventBus
    {
        private readonly ILogger<ConsumeMq> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private string AUTOFAC_SCOPE_NAME = string.Empty;
        private const string RETRY_EXCHANGE = "RETRY";
        private const string RETRY_QUEUE = "RETRY";
        private int MAX_RETRY_ATTEMPT = 5;
        private int RETRY_DELAY = 10000;

        public int retryCount { get; set; }
        readonly IModel channel;


        public ConsumeMq(IMqConnection connection, ILogger<ConsumeMq> logger, IEventBusSubscriptionsManager subsManager, ILifetimeScope autofac, int rCount = 5)
        {

            _logger = logger;
            _subsManager = subsManager;
            _autofac = autofac;
            retryCount = rCount;
            this.channel = connection.CreateModel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        /// <param name="exchangeName">Set up exchange name</param>
        /// <param name="queueName">Set up queue name</param>
        /// <param name="routingKey">Set up route key</param>
        public void Subscribe<T, TH>(string exchangeName, string queueName, string routingKey)
       where T : IntegrationEvent
       where TH : IEventHandler<T>
        {
            try
            {
                AUTOFAC_SCOPE_NAME = exchangeName;

                this.channel.QueueDeclare(queue: queueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

                channel.QueueBind(queue: queueName,
                   exchange: exchangeName,
                   routingKey: routingKey
                   );

                this.channel.BasicQos(prefetchSize: 0,
                               prefetchCount: 1,
                               global: false); // Not to give more then 1 message to worker at a time

                _subsManager.AddSubscription<T, TH>(routingKey);

                var consumer = new AsyncEventingBasicConsumer(this.channel);

                var k = this.channel;

                consumer.Received += async (o, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);

                    var i = await ProcessEventAsync(message, ea.RoutingKey).ConfigureAwait(true);

                    switch (i)
                    {
                        case ((int)BrokerAction.ConfirmAndAcknowledge):
                            {
                                // positively acknowledge all deliveries up to  this delivery tag
                                this.channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: true);
                                break;
                            }

                        case (int)BrokerAction.NegetiveAcknowledgedAndRequeue:
                            {
                                // requeue all unacknowledged deliveries up to  this delivery tag  
                                this.channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: true, requeue: true);

                                break;
                            }

                        case (int)BrokerAction.NegetiveAcknowledgedAndDiscard:
                            {
                                //Dead letter
                                HandleMessage(this.channel, queueName, routingKey, ea.Body);
                                // requeue all unacknowledged deliveries up to  this delivery tag  
                                this.channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: true, requeue: false);

                                break;
                            }

                        case (int)BrokerAction.RejectAndDiscard:
                            {
                                // negatively acknowledge, the message will be discarded
                                this.channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);
                                break;
                            }

                        case (int)BrokerAction.RejectAndRequeue:
                            {
                                // negatively acknowledge, requeue the delivery
                                this.channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                                break;
                            }

                        default:
                            {
                                // positively acknowledge all deliveries up to  this delivery tag
                                this.channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: true);
                                break;
                            }
                    }

                    await Task.Delay(250).ConfigureAwait(false);
                };

                channel.BasicConsume(queue: queueName,
                                       autoAck: false,
                                       consumer: consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception found: {ex.Message}") ;
                throw;
            }
        }

        private void HandleMessage(IModel channel, string queueName, string routingKey, byte[] message)
        {
            string WORK_EXCHANGE = "WorkExchange"; // dead letter exchange
            string RETRY_QUEUE = "RetryQueue";
            int RETRY_DELAY = 10000;

            var queueArgs = new Dictionary<string, object> {
                { "x-dead-letter-exchange", WORK_EXCHANGE },
                { "x-message-ttl", RETRY_DELAY }
            };

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.ExchangeDeclare(exchange: WORK_EXCHANGE,
                                    type: ExchangeType.Direct);

            channel.QueueBind(queue: queueName, exchange: WORK_EXCHANGE, routingKey: routingKey, arguments: null);

            channel.ExchangeDeclare(exchange: RETRY_EXCHANGE,
                                    type: ExchangeType.Direct);

            channel.QueueDeclare(queue: RETRY_QUEUE, durable: true, exclusive: false, autoDelete: false, arguments: queueArgs);
            channel.QueueBind(queue: RETRY_QUEUE, exchange: RETRY_EXCHANGE, routingKey: routingKey, arguments: null);
            channel.BasicPublish(exchange: RETRY_EXCHANGE, routingKey: routingKey, mandatory: true, basicProperties: properties, body: message);
        }

        private async Task<int> ProcessEventAsync(string message, string routingKey)
        {
            var subscriptions = _subsManager.GetHandlersForEvent(routingKey);

            using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
            {
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ResolveOptional(subscription.HandlerType);
                    if (handler == null) continue;

                    var eventType = _subsManager.GetEventTypeByName(routingKey);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                    await Task.Yield();
                    return await ((Task<int>)concreteType.GetMethod("HandleAsync").Invoke(handler, new object[] { integrationEvent })).ConfigureAwait(true);
                }
            }
            await Task.Yield();
            return (int)BrokerAction.RejectAndDiscard;
        }
    }
}
