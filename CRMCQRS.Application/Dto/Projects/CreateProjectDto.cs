using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Projects.Commands.CreateProject;

namespace CRMCQRS.Application.Dto.Projects;

public class CreateProjectDto : IMapWith<CreateProjectCommand>
{
    public string Title { get; set; }
    public string? Info { get; set; }
    public Guid UserId { get; set; }
    public List<Guid> TagIds { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateProjectDto, CreateProjectCommand>();
    }
}