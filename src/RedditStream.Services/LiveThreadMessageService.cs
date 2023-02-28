using RedditStream.Domains;
using RedditStream.Repositories.Interfaces;
using RedditStream.Services.Interfaces;

namespace RedditStream.Services;

public class LiveThreadMessageService : ILiveThreadMessageService
{
    private readonly ILiveThreadMessageRepository _liveThreadMessageRepository;

    public LiveThreadMessageService(ILiveThreadMessageRepository liveThreadMessageRepository)
    {
        _liveThreadMessageRepository = liveThreadMessageRepository;
    }

    public async Task<LiveThreadMessage?> GetOneAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _liveThreadMessageRepository.GetOneAsync(id, cancellationToken);
    }

    public async Task<LiveThreadMessage> CreateOneAsync(LiveThreadMessage liveThreadMessage, CancellationToken cancellationToken = default)
    {
        return await _liveThreadMessageRepository.CreateOneAsync(liveThreadMessage, cancellationToken);
    }

}
