using BestStories.Model;
using MediatR;

namespace BestStories.Services.Queries;

public record GetBestStoriesQuery(int StoriesCount) : IRequest<IEnumerable<Story>>
{
    public int StoriesCount { get; } = StoriesCount;
}