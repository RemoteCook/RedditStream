using Azure.Core;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditStream.Services.Configuration;
using RedditStream.Services.Interfaces;

namespace RedditStream.Services;

public class QueueClientService : IQueueClientService
{
    private readonly ILogger<QueueService> _logger;
    private readonly IOptions<RedditOptions> _redditOptions;

    private QueueClient _queueClient;

    public QueueClientService(ILogger<QueueService> logger,
        IOptions<RedditOptions> redditOptions)
    {
        ArgumentNullException.ThrowIfNull(redditOptions.Value?.StorageAccountConnectionString);

        _logger = logger;
        _redditOptions = redditOptions;
    }

    private void GetQueueClientAsync(string queueName, CancellationToken cancellationToken = default)
    {
        var queueOptions = new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64
        };
        queueOptions.Retry.Delay = TimeSpan.FromSeconds(1);
        queueOptions.Retry.MaxRetries = 3;
        queueOptions.Retry.Mode = RetryMode.Exponential;

        _queueClient = new QueueClient(_redditOptions.Value.StorageAccountConnectionString, queueName, queueOptions);
    }

    public async Task CreateIfNotExistsAsync(string queueName, CancellationToken cancellationToken = default)
    {
        GetQueueClientAsync(queueName, cancellationToken);

        await _queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task SendMessageAsync(string message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
        await _queueClient.SendMessageAsync(message, visibilityTimeout: visibilityTimeout, cancellationToken: cancellationToken);
    }
}
