using CRMCQRS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMCQRS.Infrastructure.Database.Configuration;

public class ProjectTagConfiguration : IEntityTypeConfiguration<ProjectTag>
{
    public void Configure(EntityTypeBuilder<ProjectTag> item)
    {
        item.HasBaseType((Type) null);
        item.HasKey(e => new {e.ProjectsId, e.TagsId});

        item.HasOne(i => i.Project).WithMany(r => r.Tags).HasForeignKey(r => r.ProjectsId);
        item.HasOne(i => i.Tag).WithMany(r => r.Projects).HasForeignKey(r => r.TagsId);
    }
}