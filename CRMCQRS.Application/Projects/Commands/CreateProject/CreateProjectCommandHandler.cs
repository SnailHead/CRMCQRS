using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler :IRequestHandler<CreateProjectCommand, Guid>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Project> _projectRepository;

    public CreateProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _projectRepository = _unitOfWork.GetRepository<Project>();
    }

    public async Task<Guid> Handle(CreateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = new Project()
        {
            Info = request.Info,
            Title = request.Title
        };
        
        await _projectRepository.InsertAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        
        //todo написать создание пользователя в проекте с ролью менеджер проекта
        
        return project.Id;
    }
}