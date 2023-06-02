using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Database.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace CRMCQRS.Infrastructure.Database;

public class DefaultDbContext : IdentityDbContext<
    User, Role, Guid>
{
    public DefaultDbContext()
    {
        
    }

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Mission> Missions { get; set; }
    public virtual DbSet<ProjectTag> ProjectTag { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<Sprint> Sprints { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<OpenIddictApplication> OpenIddictApplications { get; set; }
    public virtual DbSet<OpenIddictAuthorization> OpenIddictAuthorizations { get; set; }
    public virtual DbSet<OpenIddictScope> OpenIddictScopes { get; set; }
    public virtual DbSet<OpenIddictToken> OpenIddictTokens { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=CRMCQRS;Integrated Security=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.UseOpenIddict<OpenIddictApplication,
            OpenIddictAuthorization,
            OpenIddictScope,
            OpenIddictToken,
            Guid>();
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ProjectTagConfiguration());
    }
    
}
