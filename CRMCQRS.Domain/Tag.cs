using Microsoft.EntityFrameworkCore;

namespace CRMCQRS.Domain;
[Index(nameof(Title), IsUnique = true)]
public class Tag
{
    public Tag()
    {
        IsVisible = true;
    }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
    public bool IsFilled { get; set; }
    public bool IsVisible { get; set; }
    public List<ProjectTag> Projects { get; set; }
    public List<Mission> Missions { get; set; }
}