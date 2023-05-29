using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Missions.Commands.DeleteMission;

public class DeleteMissionCommandHandler :IRequestHandler<DeleteMissionCommand, bool>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Mission> _missionRepository;

    public DeleteMissionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _missionRepository = _unitOfWork.GetRepository<Mission>();
    }

    public async Task<bool> Handle(DeleteMissionCommand request,
        CancellationToken cancellationToken)
    {

        var dbMission = await _missionRepository.GetFirstOrDefaultAsync(predicate: item => item.Id == request.Id);
        
        if (dbMission is null)
        {
            throw new ArgumentException($"Mission with ID {request.Id} not found");
        }
        
        _missionRepository.Delete(dbMission);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}