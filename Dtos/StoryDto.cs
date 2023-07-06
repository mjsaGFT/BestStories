using System.Text.Json.Serialization;

namespace BestStoriesAPI.Dtos
{
    public class StoryDto
    {
        [JsonPropertyName("title")] 
        public string Title { get; set; }
        
        [JsonPropertyName("url")] 
        public string Uri { get; set; }        
        
        [JsonPropertyName("time")]   
        public int Time { get; set; }
        
        [JsonPropertyName("score")]    
        public int Score { get; set; }
        
        [JsonPropertyName("by")] 
        public string PostedBy { get; set; }
        
        public int CommentCount { get; set; }
    }
}