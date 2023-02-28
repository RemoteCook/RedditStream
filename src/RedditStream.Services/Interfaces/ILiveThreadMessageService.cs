using RedditStream.Domains;

namespace RedditStream.Services.Interfaces;

public interface ILiveThreadMessageService
{
    Task<LiveThreadMessage> CreateOneAsync(LiveThreadMessage liveThreadMessage, CancellationToken cancellationToken = default);
    Task<LiveThreadMessage> GetOneAsync(int id, CancellationToken cancellationToken = default);
}