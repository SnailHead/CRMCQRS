using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Projects.Queries.GetPageProject;

namespace CRMCQRS.API.Models.Projects;

public class GetPageProjectDto : IMapWith<GetPageProjectQuery>
{
    public string Title { get; set; }
    public int Page { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetPageProjectDto, GetPageProjectQuery>();
    }
}