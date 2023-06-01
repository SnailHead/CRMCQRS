using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMCQRS.Domain;

public class User : IdentityUser<Guid> 
{
    public User()
    {
        Permissions = new List<Permission>();
    }
    public bool IsBlocked { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Middlename { get; set; }

    public DateTime? BirthDate { get; set; }

    public int DepartmentId { get; set; }
    public long TelegramChatId { get; set; }

    public bool IsOnline { get; set; }
    public bool IsVisible { get; set; }
    /// <summary>
    /// Application permission for policy-based authorization
    /// </summary>
    public List<Permission>? Permissions { get; set; }
    public ICollection<UserMission> Missions { get; set; }
    public ICollection<OfficeTimer> OfficeTimers { get; set; }
}