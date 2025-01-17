using System.Collections.Concurrent;
using System.Text.Json;
using AutoMapper;
using BestStories.Model;
using BestStories.Model.Configuration;
using BestStories.Model.Dto;
using BestStories.Services.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BestStories.Services.Handlers;

public class GetBestStoriesHandler(IOptions<StoriesSettings> options, IMapper mapper, HttpClient client, ILogger<GetBestStoriesHandler> logger)
    : IRequestHandler<GetBestStoriesQuery, IEnumerable<Story>>
{
    private readonly StoriesSettings _options = options.Value;

    public async Task<IEnumerable<Story>> Handle(GetBestStoriesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Story> result = new List<Story>();

        var requestedIds = (await GetResponse<IEnumerable<int>>(_options.IdsUri, cancellationToken))
            .Take(request.StoriesCount)
            .ToList();

        if (requestedIds.Count <= 0) return result;

        ConcurrentBag<Story> bag = new ConcurrentBag<Story>();

        await Parallel.ForEachAsync(
            requestedIds,
            new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = _options.ParallelRequestsLimit
            },
            async (id, cancelToken) =>
            {
                StoryDto story = await GetResponse<StoryDto>(string.Format(_options.DetailUri, id), cancelToken);
                if (story != null)
                    bag.Add(mapper.Map<Story>(story));
            });

        return bag.OrderByDescending(x => x.Score).ToList();
    }

    private async Task<T> GetResponse<T>(string uri, CancellationToken cancellationToken)
    {
        T result = default;
        try
        {
            await using Stream stream = await client.GetStreamAsync(uri, cancellationToken);
            result = await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
        return result;
    }
}