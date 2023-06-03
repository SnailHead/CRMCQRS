using Microsoft.AspNetCore.Identity;

namespace CRMCQRS.Domain;

public class Role: IdentityRole<Guid>
{
    public bool IsVisible { get; set; } = true;
    public List<User> Users { get; set; }


    public Role() : base()
    {
        Id = Guid.NewGuid();
    }

    public Role(string roleName) : base(roleName)
    {
        Id = Guid.NewGuid();
        Name = roleName;
    }
}