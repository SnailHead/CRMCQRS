using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Missions.Commands.UpdateMission;
using CRMCQRS.Domain.Common.Enums;

namespace CRMCQRS.API.Models.Missions;

public class UpdateMissionDto : IMapWith<UpdateMissionCommand>
{
    public Guid Id { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? ParentId { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public MissionStatus Status { get; set; }
    public MissionPriority MissionPriority { get; set; }
}