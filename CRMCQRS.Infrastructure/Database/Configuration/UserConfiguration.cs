using CRMCQRS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMCQRS.Infrastructure.Database.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> item)
    {
        item.HasBaseType((Type) null);

        item.HasMany(i => i.AuthorMissions).WithOne(r => r.Author).HasForeignKey(r => r.AuthorId);
        item.HasMany(i => i.ExecutorMissions).WithOne(r => r.Executor).HasForeignKey(r => r.ExecutorId);
    }
}