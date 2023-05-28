using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Tags.Commands.CreateTag;

public class CreateTagCommandHandler :IRequestHandler<CreateTagCommand, Guid>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Tag> _tagRepository;

    public CreateTagCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _tagRepository = _unitOfWork.GetRepository<Tag>();
    }

    public async Task<Guid> Handle(CreateTagCommand request,
        CancellationToken cancellationToken)
    {
        var tag = new Tag()
        {
            Color = request.Color,
            IsFilled = request.IsFilled,
            Title = request.Title
        };
        
        await _tagRepository.InsertAsync(tag, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        
        return tag.Id;
    }
}