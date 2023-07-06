using System.Collections.Generic;
using BestStoriesAPI.Dtos;
using MediatR;

namespace BestStoriesAPI.Handlers
{
    public class GetBestStoriesQuery : IRequest<IEnumerable<StoryDto>>
    {
        public int StoriesCount { get; }
        public GetBestStoriesQuery(int storiesCount)
        {
            StoriesCount = storiesCount;            
        }
    }
}