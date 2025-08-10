namespace TaskManagementServiceLoging.Application.Interfaces
{
    public interface IMessageRawConsumer
    {
        Task<string> ConsumeAsync();
        Task CommitDataAsync();
    }
}