using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Tags.Commands.UpdateTag;

public class UpdateTagCommandHandler :IRequestHandler<UpdateTagCommand, bool>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Tag> _tagRepository;

    public UpdateTagCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _tagRepository = _unitOfWork.GetRepository<Tag>();
    }

    public async Task<bool> Handle(UpdateTagCommand request,
        CancellationToken cancellationToken)
    {
        bool exists = await _tagRepository.ExistsAsync(item => item.Id == request.Id, cancellationToken);
        if (!exists)
        {
            throw new ArgumentException($"Tag with ID {request.Id} not found");
        }
        var tag = new Tag()
        {
            Id = request.Id,
            Color = request.Color,
            IsFilled = request.IsFilled,
            Title = request.Title
        };
        
        _tagRepository.Update(tag);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}