using System.Collections.Generic;
using BestStoriesAPI.Dto;
using MediatR;

namespace BestStoriesAPI.Handlers;

public record GetBestStoriesQuery(int StoriesCount) : IRequest<IEnumerable<StoryOutDto>>
{
    public int StoriesCount { get; } = StoriesCount;
}