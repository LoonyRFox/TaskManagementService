namespace TaskManagementServiceLoging.Application.Interfaces;

public interface IConsumerConfig
{
    public string Topic { get; set; }
    public int RetryOnEmptyDelayMs { get; set; }
    public int RetryOnFailedDelayMs { get; set; }
    public int ConsumeTimeoutMs { get; set; }
}