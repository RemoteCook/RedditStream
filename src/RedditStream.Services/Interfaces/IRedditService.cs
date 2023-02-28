namespace RedditStream.Services.Interfaces
{
    public interface IRedditService
    {
        Task Run(CancellationToken cancellationToken = default);
    }
}