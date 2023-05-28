using CRMCQRS.Domain.Common.Enums;

namespace CRMCQRS.Domain;

public class UserMission
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MissionId { get; set; }
    public User User { get; set; }
    public Mission Mission { get; set; }
    public RoleInMission RoleInMission { get; set; }
}