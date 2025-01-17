using System.Text.Json.Serialization;

namespace BestStories.Model.Dto;

public class StoryDto
{
    [JsonPropertyName("title")] 
    public string Title { get; set; }
        
    [JsonPropertyName("url")] 
    public string Uri { get; set; }

    [JsonPropertyName("by")]
    public string By { get; set; }

    [JsonPropertyName("time")]   
    public int Time { get; set; }
        
    [JsonPropertyName("score")]    
    public int Score { get; set; }

    [JsonPropertyName("kids")]
    public List<int> Kids { get; set; }
}