using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Projects.Commands.UpdateProject;

namespace CRMCQRS.Application.Dto.Projects;

public class UpdateProjectDto: IMapWith<UpdateProjectCommand>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Info { get; set; }
    public List<Guid> TagIds { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateProjectDto, UpdateProjectCommand>();
    }
}