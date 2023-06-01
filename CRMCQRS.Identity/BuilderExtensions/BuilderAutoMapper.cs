using System.Reflection;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Infrastructure.Database;

namespace CRMCQRS.Identity.BuilderExtensions;

public static class BuilderAutoMapper
{
    public static void ConfigureMapperServicesAsync(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(DefaultDbContext).Assembly));
        });
    }
    
    public static void ConfigureMapperApplication(this WebApplication app)
    {
        var mapper = app.Services.GetRequiredService<AutoMapper.IConfigurationProvider>();
        if (app.Environment.IsDevelopment())
        {
            mapper.AssertConfigurationIsValid();
        }
        else
        {
            mapper.CompileMappings();
        }
    }
    
}