using MediatR;

namespace CRMCQRS.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommand :  IRequest<bool>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Info { get; set; }
}