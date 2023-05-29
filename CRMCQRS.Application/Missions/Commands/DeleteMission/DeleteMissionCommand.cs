using MediatR;

namespace CRMCQRS.Application.Missions.Commands.DeleteMission;

public class DeleteMissionCommand :  IRequest<bool>
{
    public Guid Id { get; set; }
    
}