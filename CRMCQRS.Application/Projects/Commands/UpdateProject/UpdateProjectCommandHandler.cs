using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler :IRequestHandler<UpdateProjectCommand, bool>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Project> _projectRepository;

    public UpdateProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _projectRepository = _unitOfWork.GetRepository<Project>();
    }

    public async Task<bool> Handle(UpdateProjectCommand request,
        CancellationToken cancellationToken)
    {
        bool exists = await _projectRepository.ExistsAsync(item => item.Id == request.Id, cancellationToken);
        if (!exists)
        {
            throw new ArgumentException($"Project with ID {request.Id} not found");
        }
        var project = new Project()
        {
            Id = request.Id,
            Info = request.Info,
            Title = request.Title
        };
        
        _projectRepository.Update(project);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}