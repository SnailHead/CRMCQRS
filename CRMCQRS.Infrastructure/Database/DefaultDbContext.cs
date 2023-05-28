using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Database.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    public virtual DbSet<OfficeTimer> OfficeTimers { get; set; }
    public virtual DbSet<ProjectTag> ProjectTag { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=CRMCQRS;Integrated Security=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ProjectTagConfiguration());
    }
}
