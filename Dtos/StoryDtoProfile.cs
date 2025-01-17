namespace BestStoriesAPI.Dto;

public class StoryDtoProfile : AutoMapper.Profile
{

    public StoryDtoProfile()
    {
        CreateMap<StoryInDto, StoryOutDto>()
            .ForMember(dst => dst.PostedBy, opts => opts.MapFrom(src => src.By))
            .ForMember(dst => dst.CommentCount, opt => opt.MapFrom(src => src.Kids.Count))
            ;
    }
}