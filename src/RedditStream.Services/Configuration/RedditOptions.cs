namespace RedditStream.Services.Configuration;

public class RedditOptions
{
    public string StorageAccountConnectionString { get; set; }
    public LiveThreadOptions LiveThread { get; set; }
    public ApiOptions Api { get; set; }
}
