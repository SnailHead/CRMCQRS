using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Tags.Commands.DeleteTag;

public class DeleteTagCommandHandler :IRequestHandler<DeleteTagCommand, bool>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Tag> _tagRepository;

    public DeleteTagCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _tagRepository = _unitOfWork.GetRepository<Tag>();
    }

    public async Task<bool> Handle(DeleteTagCommand request,
        CancellationToken cancellationToken)
    {

        var dbTag = await _tagRepository.GetFirstOrDefaultAsync(predicate: item => item.Id == request.Id);
        
        if (dbTag is null)
        {
            throw new ArgumentException($"Tag with ID {request.Id} not found");
        }
        
        _tagRepository.Delete(dbTag);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}