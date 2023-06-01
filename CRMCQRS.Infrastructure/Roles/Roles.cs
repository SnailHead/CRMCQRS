namespace CRMCQRS.Infrastructure.Roles;

public static class Roles
{
    public const string User = "user";
    public const string Admin = "admin";
    public const string Developer = "dev";
    
    public static List<string> RoleNames = new List<string>()
    {
        User, Admin, Developer
    };
}