using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BestStoriesAPI.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BestStoriesAPI.Handlers;

public class GetBestStoriesHandler(IOptions<StoriesSettings> options, IMapper mapper, HttpClient client, ILogger<GetBestStoriesHandler> logger)
    : IRequestHandler<GetBestStoriesQuery, IEnumerable<StoryOutDto>>
{
    private readonly StoriesSettings _options = options.Value;

    public async Task<IEnumerable<StoryOutDto>> Handle(GetBestStoriesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<StoryOutDto> result = new List<StoryOutDto>();

        var requestedIds = (await GetResponse<IEnumerable<int>>(_options.IdsUri, cancellationToken))
            .Take(request.StoriesCount)
            .ToList();

        if (requestedIds.Count <= 0) return result;

        ConcurrentBag<StoryOutDto> bag = new ConcurrentBag<StoryOutDto>();

        await Parallel.ForEachAsync(
            requestedIds, 
            new ParallelOptions
            {
                CancellationToken = cancellationToken, 
                MaxDegreeOfParallelism = _options.ParallelRequestsLimit
            }, 
            async (id, cancelToken) =>
            {
                StoryInDto story = await GetResponse<StoryInDto>(String.Format(_options.DetailUri, id), cancelToken);
                if (story != null)
                    bag.Add(mapper.Map<StoryOutDto>(story));
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