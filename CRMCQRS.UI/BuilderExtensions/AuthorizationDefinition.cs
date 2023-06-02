using System.Text.Json;
using CRMCQRS.Infrastructure.Authentication.Policies.Schemes;
using CRMCQRS.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Server.AspNetCore;

namespace CRMCQRS.UI.BuilderExtensions;

/// <summary>
/// Authorization Policy registration
/// </summary>
public static class AuthorizationBuilder
{

    public static void ConfigureAuthorizationServices(this IServiceCollection services)
    {
        var identityConfiguration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("identitysetting.json")
            .Build();
        
        var url = identityConfiguration.GetSection("IdentityServerUrl").GetValue<string>("Authority");
        var currentClient = identityConfiguration.GetSection("CurrentIdentityClient").Get<IdentityClientOption>()!;
        
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
        
    }
    
    public static void ConfigureAuthorizationApplication(this WebApplication app)
    {
        app.UseRouting();
        app.UseHttpsRedirection();
        app.MapDefaultControllerRoute();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapRazorPages();
    }
}