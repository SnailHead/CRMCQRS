using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Projects.Queries.GetPageProject;

public class GetPageProjectQueryHandler : IRequestHandler<GetPageProjectQuery, IPagedList<ProjectViewModel>>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;


    public GetPageProjectQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _projectRepository = _unitOfWork.GetRepository<Project>();
    }

    public async Task<IPagedList<ProjectViewModel>> Handle(GetPageProjectQuery request,
        CancellationToken cancellationToken)
    {
        var projects =
            await _projectRepository.GetPagedListAsync(predicate: request.GetExpression(request), pageIndex: request.Page,
                disableTracking: true, cancellationToken: cancellationToken, 
                selector: item => _mapper.Map<ProjectViewModel>(item));

        return projects;
    }
}