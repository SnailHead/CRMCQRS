using MediatR;

namespace CRMCQRS.Application.Tags.Commands.CreateTag;

public class CreateTagCommand :  IRequest<Guid>
{
    public string Title { get; set; }
    public string Color { get; set; }
    public bool IsFilled { get; set; }
}