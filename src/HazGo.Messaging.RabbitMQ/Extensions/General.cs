namespace HazGo.Messaging.RabbitMQ
{
    public enum BrokerAction1
    {
        ConfirmAndAcknowledge = 0,
        RejectAndDiscard = 1,
        RejectAndRequeue = 2,
        NegetiveAcknowledgedAndRequeue = 3,
        NegetiveAcknowledgedAndDiscard = 4
    }
}
