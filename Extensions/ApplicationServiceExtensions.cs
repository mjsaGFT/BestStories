using System;
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
        return services;
    }
}