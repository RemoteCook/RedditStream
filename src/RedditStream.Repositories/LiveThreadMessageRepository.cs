using Microsoft.EntityFrameworkCore;
using RedditStream.Domains;
using RedditStream.Repositories.Data;
using RedditStream.Repositories.Interfaces;

namespace RedditStream.Repositories;

public class LiveThreadMessageRepository : ILiveThreadMessageRepository
{
    private readonly RedditStreamContext _dbContext;

    public LiveThreadMessageRepository(RedditStreamContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
    }

    public async Task<LiveThreadMessage?> GetOneAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LiveThreadMessages.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<LiveThreadMessage?> CreateOneAsync(LiveThreadMessage liveThreadMessage, CancellationToken cancellationToken = default)
    {
        await _dbContext.LiveThreadMessages.AddAsync(liveThreadMessage, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return liveThreadMessage;
    }
}