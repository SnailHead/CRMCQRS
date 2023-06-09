using CRMCQRS.Domain.Common.Enums;

namespace CRMCQRS.Domain;

public class Mission
{
    public Mission()
    {
        SubMissions = new();
        Tags = new();
        IsVisible = true;
    }
    public Guid Id { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? ExecutorId { get; set; }
    public Guid? AuthorId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public MissionStatus Status { get; set; }
    public MissionType Type { get; set; }
    public MissionPriority MissionPriority { get; set; }
    public bool IsVisible { get; set; }
    
    public Project Project { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PeriodExecution { get; set; }
    public List<Mission> SubMissions { get; set; }
    public User Executor { get; set; }
    public User Author { get; set; }
    public List<Tag> Tags { get; set; }
}