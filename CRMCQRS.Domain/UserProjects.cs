using CRMCQRS.Domain.Common.Enums;

namespace CRMCQRS.Domain;

public class UserProjects
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public RoleInProject RoleInProject { get; set; }
    public User User { get; set; }
    public Project Project { get; set; }
}