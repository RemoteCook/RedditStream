namespace RedditStream.Services.Models;

public class LiveThread
{
    public string kind { get; set; }

    public LiveThreadData data { get; set; }
}

public class LiveThreadData
{
    public string websocket_url { get; set; }
}
