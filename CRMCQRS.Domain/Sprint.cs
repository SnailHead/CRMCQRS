namespace CRMCQRS.Domain;

public class Sprint
{
    public Sprint()
    {
        Project = new();
    }
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Project Project { get; set; }
}