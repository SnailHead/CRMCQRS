using CRMCQRS.Application.Missions.Queries;
using MediatR;

namespace CRMCQRS.Application.Missions.Queries.GetMission;

public class GetMissionQuery : IRequest<MissionViewModel>
{
    public Guid Id { get; set; }
}