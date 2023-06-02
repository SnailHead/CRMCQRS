using CRMCQRS.Identity.Application.Services;
using CRMCQRS.Infrastructure.Authentication.Policies.Schemes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CRMCQRS.Identity.BuilderExtensions;

public static class BuilderBase
{
    public static void ConfigureBaseServices(this IServiceCollection services)
    {
        //services.AddHttpContextAccessor();
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options => { options.LoginPath = "/account/login"; });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthSchemes.AuthenticationSchemes, policy =>
            {
                policy.RequireAuthenticatedUser();
                //policy.RequireClaim("scope", "api");
            });
        });

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<ApplicationUserClaimsPrincipalFactory>();
    }

    public static void ConfigureBaseApplication(this WebApplication app)
    {
        app.UseRouting();
        app.UseHttpsRedirection();
        app.MapDefaultControllerRoute();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapRazorPages();
    }
}