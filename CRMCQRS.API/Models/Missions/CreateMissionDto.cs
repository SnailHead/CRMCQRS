using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Missions.Commands.CreateMission;
using CRMCQRS.Application.Projects.Commands.CreateProject;
using CRMCQRS.Domain.Common.Enums;

namespace CRMCQRS.API.Models.Missions;

public class CreateMissionDto : IMapWith<CreateMissionCommand>
{
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
        profile.CreateMap<CreateMissionDto, CreateMissionCommand>();
    }
}