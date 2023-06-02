using CRMCQRS.Domain.Common.Enums;

namespace CRMCQRS.Domain;

public class Mission
{
    public Mission()
    {
        SubMissions = new();
        UserMissions = new();
        Tags = new();
    }
    public Guid Id { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? ParentId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public MissionStatus Status { get; set; }
    public MissionPriority MissionPriority { get; set; }
    public bool IsVisible { get; set; } = true;
    
    public Project Project { get; set; }
    public List<Mission> SubMissions { get; set; }
    public List<UserMission> UserMissions { get; set; }
    public List<Tag> Tags { get; set; }
}