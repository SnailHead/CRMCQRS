using MediatR;

namespace CRMCQRS.Application.Projects.Queries.GetProject;

public class GetProjectQuery :  IRequest<ProjectViewModel>
{
    public Guid Id { get; set; }
}