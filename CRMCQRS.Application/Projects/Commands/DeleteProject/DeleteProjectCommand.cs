using MediatR;

namespace CRMCQRS.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommand :  IRequest<bool>
{
    public Guid Id { get; set; }
    
}