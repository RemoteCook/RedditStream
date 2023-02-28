using Microsoft.Extensions.Logging;
using RedditStream.Services.Exceptions;
using System.Text;
using System.Text.Json;

namespace RedditStream.Services.Clients;

public class ClientBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ClientBase> _logger;
    private readonly string _relativeBaseUrl;

    public ClientBase(HttpClient httpClient,
        ILogger<ClientBase> logger,
        string relativeBaseUrl)
    {
        _httpClient = httpClient;
        _logger = logger;
        _relativeBaseUrl = relativeBaseUrl;
    }

    public async Task<T> Get<T>(string url, CancellationToken cancellationToken = default)
    {
        //HttpResponseMessage response = await _httpClient.GetAsync($"{_relativeBaseUrl}/{url}", cancellationToken);
        HttpResponseMessage response = await _httpClient.GetAsync($"{url}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogError("Error calling Endpoint: GET {@Endpoint}. Response: {@Response}.",
                response.RequestMessage.RequestUri, responseMessage);

            throw new ClientException(response.StatusCode, responseMessage);
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(content);
    }

    public async Task<TOut> Post<TIn, TOut>(string url, TIn data, CancellationToken cancellationToken = default)
    {
        var stringContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync($"{_relativeBaseUrl}/{url}", stringContent, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var requestMessage = await response.RequestMessage.Content.ReadAsStringAsync(cancellationToken);

            var responseMessage = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogError("Error calling Endpoint: POST {@Endpoint}. Request: {@Request}, Response: {@Response}.",
                response.RequestMessage.RequestUri, requestMessage, responseMessage);

            throw new ClientException(response.StatusCode, responseMessage);
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonSerializer.Deserialize<TOut>(content);
    }
}
