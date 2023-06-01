using CRMCQRS.Identity.Application.Services;

namespace CRMCQRS.Identity.BuilderExtensions;

public static class BuilderBase
{
    public static void ConfigureBaseServices(this IServiceCollection services)
    {
        //services.AddHttpContextAccessor();
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AddAuthentication().AddCookie();

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<ApplicationUserClaimsPrincipalFactory>();
    }

    public static void ConfigureBaseApplication(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.MapDefaultControllerRoute();
        app.MapRazorPages();
    }
}