using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BestStoriesAPI.Dtos;
using MediatR;
using Microsoft.Extensions.Options;

namespace BestStoriesAPI.Handlers
{
    public class GetBestStoriesHandler : IRequestHandler<GetBestStoriesQuery, IEnumerable<StoryDto>>
    {
        private readonly StoriesSettings _options;
        public GetBestStoriesHandler(IOptions<StoriesSettings> options)
        {
            _options = options.Value;            
        }

        public async Task<IEnumerable<StoryDto>> Handle(GetBestStoriesQuery request, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();

            var requestedIds = await GetBestStoriesIds(client, request, cancellationToken);

            ConcurrentBag<StoryDto> bag = new ConcurrentBag<StoryDto>();
            await Parallel.ForEachAsync(requestedIds, async (id, cancellationToken) => 
            {
                var story = await GetStoryDetails(client,id, cancellationToken);
                bag.Add(story);
            });

            return bag.OrderByDescending(x=> x.Score).ToList();
        }

        private async Task<StoryDto> GetStoryDetails(HttpClient client, int id, CancellationToken cancellationToken)
        {                
            string uri = String.Format(_options.DetailURI,id);
            var response = await client.GetAsync(uri, cancellationToken);
            var result = await response.Content.ReadAsStringAsync();
            var story = JsonSerializer.Deserialize<StoryDto>(result);
            story.CommentCount = CalculateCommentCount(result);
            return story;
        }

        private async Task<IEnumerable<int>> GetBestStoriesIds(HttpClient client, GetBestStoriesQuery request, CancellationToken cancellationToken)
        {
            await using Stream stream = await client.GetStreamAsync(_options.IdsURI, cancellationToken);
            var ids = await JsonSerializer.DeserializeAsync<List<int>>(stream, cancellationToken: cancellationToken);
            return ids.Take(request.StoriesCount);
        }
        
        private int CalculateCommentCount(string response)
        {
            //TODO: experiment with converters.
            return response.Split("[")[1].Split("]")[0].Split(",").Count();
        }
    }
}