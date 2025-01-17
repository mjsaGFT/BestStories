using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using BestStories.Model.Dto;
using BestStories.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BestStoriesAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[ResponseCache(CacheProfileName = "Default30")]
public class BestStoriesController(ILogger<BestStoriesController> logger, IMediator mediator) : ControllerBase
{
    [HttpGet("{storiesCount}")]
    [ProducesResponseType(typeof(IEnumerable<StoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStories(int storiesCount, CancellationToken cancellationToken)
    {
        try
        {
            if (storiesCount <= 0) return BadRequest("Parameter must be greater than 0.");

            var result = await mediator.Send(new GetBestStoriesQuery(storiesCount), cancellationToken);
            return Ok(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = e.Message });
        }
    }
}
