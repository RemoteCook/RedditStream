using Microsoft.Extensions.Logging;
using RedditStream.Services.Models;

namespace RedditStream.Services.Clients;

public class RedditClient : ClientBase, IRedditClient
{
    const string API_VERSION = "v1";

    public RedditClient(HttpClient httpClient, ILogger<RedditClient> logger)
        : base(httpClient, logger, $"api/{API_VERSION}")
    {


    }

    public async Task<string> GetMe(CancellationToken cancellationToken = default)
    {
        return await Get<string>("api/v1/me", cancellationToken);
    }

    public async Task<LiveThread> GetLiveThread(string liveThreadId, CancellationToken cancellationToken = default)
    { 
        return await Get<LiveThread>($"live/{liveThreadId}/about.json", cancellationToken);

    }
}
