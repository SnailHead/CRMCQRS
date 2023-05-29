using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Missions.Commands.CreateMission;
using CRMCQRS.Domain;
using CRMCQRS.Domain.Common.Enums;
using MediatR;

namespace CRMCQRS.Application.Missions.Commands.UpdateMission;

public class UpdateMissionCommand :  IRequest<bool>, IMapWith<Mission>
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
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateMissionCommand, Mission>();
    }
}