using MediatR;

namespace CRMCQRS.Application.Tags.Commands.UpdateTag;

public class UpdateTagCommand :  IRequest<bool>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
    public bool IsFilled { get; set; }
}