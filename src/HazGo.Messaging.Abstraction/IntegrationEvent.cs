namespace HazGo.Messaging.Abstraction
{
    using System;
    using Newtonsoft.Json;
    public class IntegrationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEvent"/> class.
        /// Initialize Event 
        /// </summary>
        public IntegrationEvent()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEvent"/> class.
        /// Constructor parameteres.
        /// </summary>
        /// <param name="id">Id for event</param>
        /// <param name="createDate">Creation date for event.</param>
        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            this.Id = id;
            this.CreationDate = createDate;
        }

        /// <summary>
        /// Gets or sets id for the event.
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets event creation date.
        /// </summary>
        [JsonProperty]
        public DateTime CreationDate { get; set; }

        [JsonProperty]
        public long PublishDate { get; set; }
    }
}

