using CRMCQRS.Domain.Common.Enums;

namespace CRMCQRS.Domain;

public class Mission
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
    public bool IsVisible { get; set; } = true;
    
    public Project Project { get; set; }
    public ICollection<Mission> SubMissions { get; set; } = new List<Mission>();
    public ICollection<UserMission> UserMissions { get; set; } = new List<UserMission>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}