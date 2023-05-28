using MediatR;

namespace CRMCQRS.Application.Tags.Queries.GetTag;

public class GetTagQuery : IRequest<TagViewModel>
{
    public Guid Id { get; set; }
}