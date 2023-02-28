namespace RedditStream.Services.Configuration;

public class ApiAuthenticationOptions
{
    public string BaseUrl { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string GrantType { get; set; }
}
