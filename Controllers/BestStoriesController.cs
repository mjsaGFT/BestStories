using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BestStoriesAPI.Dtos;
using BestStoriesAPI.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BestStoriesAPI.Controllers;

[ApiController]
[Route("[controller]")]
[ResponseCache(CacheProfileName = "Default30")]
public class BestStoriesController : ControllerBase
{
    private readonly ILogger<BestStoriesController> _logger;
    public IMediator _mediator { get; }
    public BestStoriesController(ILogger<BestStoriesController> logger, IMediator mediator)
    {
        _mediator = mediator;
        _logger = logger;

    }

    [HttpGet("{storiesCount}")]
    public async Task<IEnumerable<StoryDto>> GetStories(int storiesCount, CancellationToken cancellationToken)
    {            
        return await _mediator.Send(new GetBestStoriesQuery(storiesCount));
    } 
}
