using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Projects.Queries;
using CRMCQRS.Application.Users.Queries;
using CRMCQRS.Domain;
using CRMCQRS.Domain.Common.Enums;

namespace CRMCQRS.Application.Missions.Queries;

public class MissionViewModel : IMapWith<Mission>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public MissionStatus Status { get; set; }
    public MissionPriority MissionPriority { get; set; }
    public List<UserViewModel> Users { get; set; }
    public ProjectViewModel Project { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Mission, MissionViewModel>();
    }
}