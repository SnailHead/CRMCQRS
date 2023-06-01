namespace CRMCQRS.Domain;

/// <summary>
/// User permission for application
/// </summary>
public class Permission
{
    public Guid Id { get; set; }

    /// <summary>
    /// Application User Profile
    /// </summary>
    public virtual List<User>? ApplicationUserProfiles { get; set; }

    /// <summary>
    /// Authorize attribute policy name
    /// </summary>
    public string PolicyName { get; set; } = null!;

    /// <summary>
    /// Description for current permission
    /// </summary>
    public string Description { get; set; } = null!;
}