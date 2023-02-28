using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedditStream.Services.Configuration;
using RedditStream.Services.Exceptions;
using RedditStream.Services.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RedditStream.Services.Clients;

public class RedditClientAuthentication : IRedditClientAuthentication
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<RedditOptions> _redditOptions;
    private readonly ILogger<RedditClientAuthentication> _logger;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _username;
    private readonly string _password;
    private readonly string _grantType;

    public RedditClientAuthentication(
        HttpClient httpClient,
        IOptions<RedditOptions> redditOptions,
        ILogger<RedditClientAuthentication> logger)
    {
        _httpClient = httpClient;
        _redditOptions = redditOptions;
        _logger = logger;

        MyArgumentNullException.ThrowIfNullOrEmpty(_redditOptions.Value?.Api.Authentication.ClientId);
        MyArgumentNullException.ThrowIfNullOrEmpty(_redditOptions.Value?.Api.Authentication.ClientSecret);
        MyArgumentNullException.ThrowIfNullOrEmpty(_redditOptions.Value?.Api.Authentication.Username);
        MyArgumentNullException.ThrowIfNullOrEmpty(_redditOptions.Value?.Api.Authentication.Password);
        MyArgumentNullException.ThrowIfNullOrEmpty(_redditOptions.Value?.Api.Authentication.GrantType);

        _clientId = _redditOptions.Value.Api.Authentication.ClientId;
        _clientSecret = _redditOptions.Value.Api.Authentication.ClientSecret;
        _username = _redditOptions.Value.Api.Authentication.Username;
        _password = _redditOptions.Value.Api.Authentication.Password;
        _grantType = _redditOptions.Value.Api.Authentication.GrantType;
    }

    public async Task<TokenResponse> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var authToken = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

        var @params = new Dictionary<string, string>
        {
            { "grant_type", _grantType },
            { "username", _username },
            { "password", _password }
        };

        HttpResponseMessage response = await _httpClient.PostAsync("api/v1/access_token", new FormUrlEncodedContent(@params), cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            string responseMessage = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Error calling Endpoint: GET {@Endpoint}. Response: {@Response}.",
                   response.RequestMessage.RequestUri, responseMessage);

            throw new Exception();
        }

        string content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<TokenResponse>(content);
    }
}
