using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler :IRequestHandler<DeleteProjectCommand, bool>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Project> _projectRepository;

    public DeleteProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _projectRepository = _unitOfWork.GetRepository<Project>();
    }

    public async Task<bool> Handle(DeleteProjectCommand request,
        CancellationToken cancellationToken)
    {

        var dbProject = await _projectRepository.GetFirstOrDefaultAsync(predicate: item => item.Id == request.Id);
        
        if (dbProject is null)
        {
            throw new ArgumentException($"Project with ID {request.Id} not found");
        }
        
        _projectRepository.Delete(dbProject);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}