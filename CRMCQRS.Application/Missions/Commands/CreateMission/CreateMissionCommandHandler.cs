using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Missions.Commands.CreateMission;

public class CreateMissionCommandHandler :IRequestHandler<CreateMissionCommand, Guid>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepository<Mission> _missionRepository;

    public CreateMissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _missionRepository = _unitOfWork.GetRepository<Mission>();
    }

    public async Task<Guid> Handle(CreateMissionCommand request,
        CancellationToken cancellationToken)
    {
        var mission = _mapper.Map<Mission>(request);
        await _missionRepository.InsertAsync(mission, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        
        return mission.Id;
    }
}