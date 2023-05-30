using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Tags.Queries.GetPageTag;

namespace CRMCQRS.Application.Dto.Tags;

public class GetPageTagDto : IMapWith<GetPageTagQuery>
{
    public GetPageTagDto(string title, int page)
    {
        Title = title;
        Page = page;
    }

    public string Title { get; set; }
    public int Page { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetPageTagDto, GetPageTagQuery>();
    }
}