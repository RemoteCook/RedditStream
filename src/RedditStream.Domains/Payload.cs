using System.Text.Json.Serialization;

namespace RedditStream.Domains;

public class Payload : DomainBase
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }
 
    [JsonPropertyName("data")]
    public Data Data { get; set; }
}
