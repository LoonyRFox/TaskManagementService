namespace TaskManagementServiceLoging.Application.Interfaces.MessageConsumerFactory;

public interface IMessageConsumerFactory
{
    IMessageConsumer CreateConsumer(string topic);
}