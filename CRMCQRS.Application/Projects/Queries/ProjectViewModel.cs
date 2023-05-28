using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Domain;

namespace CRMCQRS.Application.Projects.Queries;

public class ProjectViewModel : IMapWith<Project>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Info { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProjectViewModel, Project>();
    }
}