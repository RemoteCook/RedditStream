namespace RedditStream.Services.Interfaces;

public interface IQueueClientService
{
    Task CreateIfNotExistsAsync(string queueName, CancellationToken cancellationToken = default);

    Task SendMessageAsync(string message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default);
}