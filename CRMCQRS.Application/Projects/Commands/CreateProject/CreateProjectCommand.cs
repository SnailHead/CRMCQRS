using MediatR;

namespace CRMCQRS.Application.Projects.Commands.CreateProject;

public class CreateProjectCommand :  IRequest<Guid>
{
    public string Title { get; set; }
    public string Info { get; set; }
    public Guid UserId { get; set; }
    public List<Guid> TagIds { get; set; }
}