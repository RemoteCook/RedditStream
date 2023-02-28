namespace RedditStream.Services.Configuration;

public class ApiOptions
{
    public string BaseUrl { get; set; }
    public ApiAuthenticationOptions Authentication { get; set; }
}
