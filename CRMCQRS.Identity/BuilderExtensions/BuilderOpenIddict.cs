using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Database;
using CRMCQRS.Infrastructure.Options;
using OpenIddict.Abstractions;

namespace CRMCQRS.Identity.BuilderExtensions;

public static class BuilderOpenIddict
{
    public static void ConfigureOpenIddictServices(this IServiceCollection services)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("identitysetting.json");
        IConfiguration identityConfiguration = configurationBuilder.Build();
        
        var scopeOptions = identityConfiguration.GetSection("Scopes").Get<List<IdentityScopeOption>>()!;
        var scopes = new List<string> { OpenIddictConstants.Scopes.Email, OpenIddictConstants.Scopes.Profile, OpenIddictConstants.Scopes.Roles, };
        scopeOptions.ForEach(x => scopes.Add(x.Name));

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<DefaultDbContext>()
                    .ReplaceDefaultEntities<OpenIddictApplication,
                        OpenIddictAuthorization,
                        OpenIddictScope,
                        OpenIddictToken,
                        Guid>();
            })
            .AddServer(options =>
            {
                options
                    .AllowDeviceCodeFlow()
                    .AllowAuthorizationCodeFlow()//.RequireProofKeyForCodeExchange()
                    .AllowPasswordFlow()
                    .AllowClientCredentialsFlow()
                    .AllowRefreshTokenFlow();
                
                options.SetAccessTokenLifetime(TimeSpan.FromHours(30));
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

                options.SetAuthorizationEndpointUris("api/connect/authorize")
                    //.RequireProofKeyForCodeExchange() // enable PKCE
                    .SetDeviceEndpointUris("api/connect/device")
                    //.SetIntrospectionEndpointUris("connect/introspect")
                    .SetLogoutEndpointUris("api/connect/logout")
                    .SetTokenEndpointUris("api/connect/token")
                    .SetVerificationEndpointUris("api/connect/verify")
                    .SetUserinfoEndpointUris("api/connect/userinfo");
                
                // only for developer mode
                options
                    .AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey() 
                    .DisableAccessTokenEncryption();
                
                // server scopes
                options.RegisterScopes(scopes.ToArray());
                
                //certificate
                options
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
                
                
                // registration ASP.NET Core host and configure setting for ASP.NET Core.
                options
                    .UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer(); 
                options.UseAspNetCore();
            });
    }
}