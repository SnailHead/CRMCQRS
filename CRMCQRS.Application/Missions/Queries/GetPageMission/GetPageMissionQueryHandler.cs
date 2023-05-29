using AutoMapper;
using CRMCQRS.Application.Missions.Queries;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Missions.Queries.GetPageMission;

public class GetPageMissionQueryHandler : IRequestHandler<GetPageMissionQuery, IPagedList<MissionViewModel>>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Mission> _missionRepository;
    private readonly IMapper _mapper;


    public GetPageMissionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _missionRepository = _unitOfWork.GetRepository<Mission>();
    }

    public async Task<IPagedList<MissionViewModel>> Handle(GetPageMissionQuery request,
        CancellationToken cancellationToken)
    {
        var missions =
            await _missionRepository.GetPagedListAsync(predicate: request.GetExpression(request), pageIndex: request.Page,
                disableTracking: true, cancellationToken: cancellationToken, 
                selector: item => _mapper.Map<MissionViewModel>(item));

        return missions;
    }
}