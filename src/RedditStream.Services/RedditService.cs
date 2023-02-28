using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditStream.Domains;
using RedditStream.Services.Clients;
using RedditStream.Services.Configuration;
using RedditStream.Services.Constants;
using RedditStream.Services.Interfaces;
using RedditStream.Services.Models;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace RedditStream.Services;

public class RedditService : IRedditService
{
    private readonly IRedditClient _redditClient;
    private readonly IRedditClientAuthentication _redditClientAuthentication;
    private readonly IOptions<RedditOptions> _redditOptions;
    private readonly ILogger<RedditService> _logger;
    private readonly IQueueService _queueService;
    private readonly ILiveThreadMessageService _liveThreadMessageService;

    public RedditService(IRedditClient redditClient,
        IOptions<RedditOptions> redditOptions,
        IRedditClientAuthentication redditClientAuthentication,
        ILogger<RedditService> logger,
        IQueueService queueService,
        ILiveThreadMessageService liveThreadMessageService)
    {
        _redditClient = redditClient;
        _redditOptions = redditOptions;
        _redditClientAuthentication = redditClientAuthentication;
        _logger = logger;
        _queueService = queueService;
        _liveThreadMessageService = liveThreadMessageService;
    }

    public async Task Run(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"{nameof(RedditService)} called.");

        LiveThread liveThread = await _redditClient.GetLiveThread(_redditOptions.Value.LiveThread.Id, cancellationToken);

        //GetWebSocketUrl based on LiveThread:Id
        using var ws = new ClientWebSocket();
        await ws.ConnectAsync(new Uri(liveThread.data.websocket_url), CancellationToken.None);

        var buffer = new byte[256];
        StringBuilder stringBuilder = new();

        while (ws.State == WebSocketState.Open)
        {
            var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            }
            else
            {
                stringBuilder.Append(Encoding.ASCII.GetString(buffer, 0, result.Count));
            }

            if (result.EndOfMessage)
            {
                await Console.Out.WriteLineAsync(stringBuilder.ToString());
                _logger.LogInformation(stringBuilder.ToString());

                var message = JsonSerializer.Deserialize<LiveThreadMessage>(stringBuilder.ToString());

                //Add to storage
                await _liveThreadMessageService.CreateOneAsync(message, cancellationToken);

                //Add to queue
                //Ideally should use ServiceBus Topics instead of Storage queues for more flexibiliy with multiple subscribers and scaling
                //
                await _queueService.QueueMessageAsync(QueueNames.LiveThreadMessage, message, cancellationToken: cancellationToken);

                stringBuilder.Clear();
            }
        }
    }
}
