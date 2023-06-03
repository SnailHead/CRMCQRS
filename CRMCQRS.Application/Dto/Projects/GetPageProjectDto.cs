using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Projects.Queries.GetPageProject;

namespace CRMCQRS.Application.Dto.Projects;

public class GetPageProjectDto : IMapWith<GetPageProjectQuery>
{
    public GetPageProjectDto(){}
    public GetPageProjectDto(string title, int page)
    {
        Title = title;
        Page = page;
    }

    public string Title { get; set; }
    public int Page { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetPageProjectDto, GetPageProjectQuery>();
    }
}