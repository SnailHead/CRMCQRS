using AutoMapper;
using CRMCQRS.Application.Missions.Queries;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Missions.Queries.GetMission;

public class GetMissionQueryHandler :IRequestHandler<GetMissionQuery, MissionViewModel>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Mission> _missionRepository;
    private readonly IMapper _mapper;


    public GetMissionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _missionRepository = _unitOfWork.GetRepository<Mission>();
    }

    public async Task<MissionViewModel> Handle(GetMissionQuery request,
        CancellationToken cancellationToken)
    {
        var dbMission = await _missionRepository.GetFirstOrDefaultAsync(predicate:item => item.Id == request.Id);
        if (dbMission is null)
        {
            throw new ArgumentException($"Mission with ID {request.Id} not found");
        }

        return _mapper.Map<MissionViewModel>(dbMission);
    }
}