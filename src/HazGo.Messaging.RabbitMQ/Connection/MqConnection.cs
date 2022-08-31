/// <summary>
/// Create connection
/// </summary>
namespace HazGo.Messaging.RabbitMQ.Connection
{
    using System;
    using global::RabbitMQ.Client;

    public sealed class MqConnection : IMqConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MqConnection"/> class.
        /// Initialize Connection.
        /// </summary>
        /// <param name="connectionFactory">Connection factory. </param>
        public MqConnection(IConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public bool IsConnected
        {
            get
            {
                return this._connection != null && this._connection.IsOpen && !this._disposed;
            }
        }

        /// <summary>
        ///  Create connection & model.
        /// </summary>
        /// <returns>Connection Model</returns>

        public IModel CreateModel()
        {
            if (!this.IsConnected)
            {
                this._connection = this._connectionFactory
                         .CreateConnection();
            }

            return this._connection.CreateModel();
        }

        /// <summary>
        /// release the object.
        /// </summary>
        public void Dispose()
        {
            this._disposed = true;
            if (_connection != null)
                _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
