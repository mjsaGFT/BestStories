using BestStories.Model.Dto;

namespace BestStories.Model.Mapping;

public class StoryDtoProfile : AutoMapper.Profile
{

    public StoryDtoProfile()
    {
        CreateMap<StoryDto, Story>()
            .ForMember(dst => dst.PostedBy, opts => opts.MapFrom(src => src.By))
            .ForMember(dst => dst.CommentCount, opt => opt.MapFrom(src => src.Kids.Count))
            ;
    }
}