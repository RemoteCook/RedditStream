using System.Text.Json.Serialization;

namespace RedditStream.Domains;

public class Data :DomainBase
{
    [JsonPropertyName("body")]
    public string Body { get; set; }
 
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("author")]
    public string Author { get; set; }
    
    [JsonPropertyName("stricken")]
    public bool Stricken { get; set; }
    
    [JsonPropertyName("id")]
    public string MessageId { get; set; }
}
