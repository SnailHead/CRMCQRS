namespace CRMCQRS.Domain;

public class OfficeTimer
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public string? Report { get; set; }
    public bool IsVisible { get; set; } = true;

    public User User { get; set; }
}