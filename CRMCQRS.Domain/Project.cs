namespace CRMCQRS.Domain;

public class Project
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreationDate { get; set; }
    public string? Info { get; set; }
    public bool IsVisible { get; set; } = true;
    public List<UserProjects> Users { get; set; }
    public List<ProjectTag> Tags { get; set; } 
    public List<Mission> Missions { get; set; } 

}