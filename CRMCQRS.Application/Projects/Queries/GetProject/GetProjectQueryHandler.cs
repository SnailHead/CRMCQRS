using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Projects.Queries.GetProject;

public class GetProjectQueryHandler :IRequestHandler<GetProjectQuery, ProjectViewModel>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;


    public GetProjectQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _projectRepository = _unitOfWork.GetRepository<Project>();
    }

    public async Task<ProjectViewModel> Handle(GetProjectQuery request,
        CancellationToken cancellationToken)
    {
        var dbProject = await _projectRepository.GetFirstOrDefaultAsync(predicate:item => item.Id == request.Id);
        if (dbProject is null)
        {
            throw new ArgumentException($"Project with ID {request.Id} not found");
        }

        return _mapper.Map<ProjectViewModel>(dbProject);
    }
}