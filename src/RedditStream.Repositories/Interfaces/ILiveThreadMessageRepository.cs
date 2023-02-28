using RedditStream.Domains;

namespace RedditStream.Repositories.Interfaces;

public interface ILiveThreadMessageRepository
{
    Task<LiveThreadMessage?> CreateOneAsync(LiveThreadMessage liveThreadMessage, CancellationToken cancellationToken = default);
    Task<LiveThreadMessage?> GetOneAsync(int id, CancellationToken cancellationToken = default);
}