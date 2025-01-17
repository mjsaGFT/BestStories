using System;
using System.Collections.Generic;
using BestStories.Model;
using BestStories.Services.Handlers;
using BestStories.Services.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BestStoriesAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors();
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.AddHttpClient<IRequestHandler<GetBestStoriesQuery, IEnumerable<Story>>, GetBestStoriesHandler>();
        return services;
    }
}