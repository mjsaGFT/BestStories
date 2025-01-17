using Asp.Versioning;
using BestStoriesAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using BestStories.Model.Configuration;
using BestStories.Model.Mapping;
using BestStoriesAPI.Properties;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
});
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default30",
        new CacheProfile
        {
            Duration = 30
        });
});
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = Constants.BestStoriesAPI, Version = "v1" });
});
builder.Services.AddAutoMapper(options =>
{
    options.AddProfile(new StoryDtoProfile());
});
builder.Services.Configure<StoriesSettings>(builder.Configuration.GetSection("Stories"));
builder.Services.AddResponseCaching();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", Constants.BestStoriesAPI);
    });
}

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();
app.MapControllers();

app.Run();

