using Microsoft.Extensions.Logging;
using RedditStream.Services.Interfaces;
using System.Text.Json;

namespace RedditStream.Services;

public class QueueService : IQueueService
{
    private readonly ILogger<QueueService> _logger;
    private readonly IQueueClientService _queueClientService;

    public QueueService(ILogger<QueueService> logger,
        IQueueClientService queueClientService)
    {
        _logger = logger;
        _queueClientService = queueClientService;
    }

    public async Task QueueMessageAsync(string queueName, string message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
        await QueueMessage(queueName, message, visibilityTimeout, cancellationToken);
    }

    public async Task QueueMessageAsync<T>(string queueName, T message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
        await QueueMessage(queueName, JsonSerializer.Serialize(message), visibilityTimeout, cancellationToken);
    }

    private async Task QueueMessage(string queueName, string message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
        try
        {
            await _queueClientService.CreateIfNotExistsAsync(queueName, cancellationToken);

            await _queueClientService.SendMessageAsync(message, visibilityTimeout: visibilityTimeout, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding message {@Message} to queue {@Queue}.",
              message, queueName);

            throw;
        }
    }


}
