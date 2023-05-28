using MediatR;

namespace CRMCQRS.Application.Tags.Commands.DeleteTag;

public class DeleteTagCommand :  IRequest<bool>
{
    public Guid Id { get; set; }
    
}