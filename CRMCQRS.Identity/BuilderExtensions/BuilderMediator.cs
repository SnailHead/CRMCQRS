namespace CRMCQRS.Identity.BuilderExtensions;

/// <summary>
/// Register Mediator as application definition
/// </summary>
public static class BuilderMediator
{
    public static void ConfigureMediatorServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
    }
}