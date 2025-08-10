namespace TaskManagementServiceLoging.Application.Interfaces;

public interface IMessageConsumer
{
    Task<T> ConsumeAsync<T>();

    Task<string> ConsumeRawAsync();

    Task CommitDataAsync();
}