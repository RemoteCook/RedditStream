using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditStream.Services.Configuration;
using RedditStream.Services.Models;
using System.Net.Http.Headers;

namespace RedditStream.Services.Clients;

public class RedditAuthenticationHandler : DelegatingHandler
{
    private const string REDDIT_TOKEN_CACHE_KEY = "RedditTokenCacheKey";
    private readonly IMemoryCache _cache;
    private readonly ILogger<RedditAuthenticationHandler> _logger;
    private readonly IRedditClientAuthentication _redditClientAuthentication;
    private readonly IOptions<RedditOptions> _redditOptions;

    public RedditAuthenticationHandler(IMemoryCache cache,
        IRedditClientAuthentication redditClientAuthentication,
        IOptions<RedditOptions> redditOptions,
        ILogger<RedditAuthenticationHandler> logger)
    {
        _cache = cache;
        _redditClientAuthentication = redditClientAuthentication;
        _redditOptions = redditOptions;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("User-Agent", $"Windows RedditStream v1.0 (by /u/{_redditOptions.Value.Api.Authentication.Username})");
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetAccessTokenAsync()
    {
        if (_cache.TryGetValue(REDDIT_TOKEN_CACHE_KEY, out object? value))
        {
            return Convert.ToString(value);
        }
        try
        {
            TokenResponse tokenResponse = await _redditClientAuthentication.GetAccessTokenAsync();

            _cache.Set(REDDIT_TOKEN_CACHE_KEY, tokenResponse.AccessToken, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenResponse.ExpiresIn)
            });

            return tokenResponse.AccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"There was an issue retrieving a token from Reddit.");
            
            throw new InvalidOperationException($"There was an issue retrieving a token from Reddit.", ex);
        }
    }
}
