using System.Text.Json.Serialization;

namespace RedditStream.Domains;

public class LiveThreadMessage : DomainBase
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
 
    [JsonPropertyName("payload")]
    public Payload Payload { get; set; }
}
