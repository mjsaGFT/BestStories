using System.Text.Json.Serialization;

namespace BestStories.Model;

public class Story
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("url")]
    public string Uri { get; set; }

    [JsonPropertyName("postedBy")]
    public string PostedBy { get; set; }

    [JsonPropertyName("time")]
    public int Time { get; set; }

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("commentCount")]
    public int CommentCount { get; set; }
}
