using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRMCQRS.Application.Projects.Queries.GetPageProject;

public class GetPageProjectQueryHandler : IRequestHandler<GetPageProjectQuery, IPagedList<ProjectViewModel>>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Project> _projectRepository;
    private readonly IMapper _mapper;


    public GetPageProjectQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException("unitOfWork", "Parameter unitOfWork is null");
        _mapper = mapper ?? throw new ArgumentNullException("mapper", "Parameter mapper is null");
        _projectRepository = _unitOfWork.GetRepository<Project>();
    }

    public async Task<IPagedList<ProjectViewModel>> Handle(GetPageProjectQuery request,
        CancellationToken cancellationToken)
    {
        var projects =
            await _projectRepository.GetPagedListAsync(predicate: request.GetExpression(request),
                pageIndex: request.Page - 1,
                disableTracking: true, cancellationToken: cancellationToken,
                include: item => item.Include(x => x.Tags).ThenInclude(x => x.Tag)
                    .Include(x => x.Missions),
                selector: item => _mapper.Map<ProjectViewModel>(item));

        return projects;
    }
}