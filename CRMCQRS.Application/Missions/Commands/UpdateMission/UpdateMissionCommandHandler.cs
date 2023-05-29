using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Missions.Commands.UpdateMission;

public class UpdateMissionCommandHandler :IRequestHandler<UpdateMissionCommand, bool>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepository<Mission> _missionRepository;

    public UpdateMissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _missionRepository = _unitOfWork.GetRepository<Mission>();
    }

    public async Task<bool> Handle(UpdateMissionCommand request,
        CancellationToken cancellationToken)
    {
        bool exists = await _missionRepository.ExistsAsync(item => item.Id == request.Id, cancellationToken);
        if (!exists)
        {
            throw new ArgumentException($"Mission with ID {request.Id} not found");
        }

        var mission = _mapper.Map<Mission>(request);
        
        _missionRepository.Update(mission);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}