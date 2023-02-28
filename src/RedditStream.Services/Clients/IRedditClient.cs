using RedditStream.Services.Models;

namespace RedditStream.Services.Clients;

public interface IRedditClient
{
    Task<LiveThread> GetLiveThread(string liveThreadId, CancellationToken cancellationToken = default);
    Task<string> GetMe(CancellationToken cancellationToken = default);
}
