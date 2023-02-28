using RedditStream.Services.Models;

namespace RedditStream.Services.Clients;

public interface IRedditClientAuthentication
{
    Task<TokenResponse> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}