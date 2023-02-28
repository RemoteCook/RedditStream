namespace RedditStream.Services.Interfaces;

public interface IQueueService
{
    Task QueueMessageAsync<T>(string queueName, T message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default);
    Task QueueMessageAsync(string queueName, string message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default);
}