using CRMCQRS.Identity.BuilderExtensions.Options;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace CRMCQRS.Identity.BuilderExtensions;

public static class BuilderSwagger
{
    private const string _swaggerConfig = "/swagger/v1/swagger.json";
    public static void ConfigureSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Swagger description
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CRMCQRS Identity",
                    Version = "v1",
                });
                
                options.ResolveConflictingActions(x => x.First());

                var identityConfiguration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("identitysetting.json")
                    .Build();
                
                var url = identityConfiguration.GetSection("IdentityServerUrl").GetValue<string>("Authority");
                
                // Get scopes for AddSecurityDefinition 
                var scopesList = identityConfiguration.GetSection("Scopes").Get<List<IdentityScopeOption>>();
                var scopes = scopesList!.ToDictionary(x => x.Name, x => x.Description);

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri($"{url}/connect/token", UriKind.Absolute),
                                AuthorizationUrl = new Uri($"{url}/connect/authorize", UriKind.Absolute),
                                Scopes = scopes,
                            },
                            ClientCredentials = new OpenApiOAuthFlow
                            {
                                Scopes = scopes,
                                TokenUrl = new Uri($"{url}/connect/token", UriKind.Absolute),
                            },
                            Password = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri($"{url}/connect/token", UriKind.Absolute),
                                Scopes = scopes,
                            }
                        },
                        Type = SecuritySchemeType.OAuth2
                    }
                );
                
                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            },
                            In = ParameterLocation.Cookie,
                            Type = SecuritySchemeType.OAuth2
                        },
                        new List<string>()
                    }
                });
            });
        }

    public static void ConfigureSwaggerApplication(this WebApplication app)
    {
        /*if (!app.Environment.IsDevelopment())
        {
            return;
        }*/
            
        using var scope = app.Services.CreateAsyncScope();
        var url = scope.ServiceProvider.GetService<IOptions<IdentityAddressOption>>()!.Value.Authority;
        var client = scope.ServiceProvider.GetService<IOptions<IdentityClientOption>>()!.Value;
            
        app.UseSwagger();
        app.UseSwaggerUI(settings =>
        {
            settings.DefaultModelExpandDepth(0);
            settings.DefaultModelsExpandDepth(0);
            settings.OAuthScopeSeparator(" ");
            settings.OAuthClientId(client.Id);
            settings.OAuthClientSecret(client.Secret);
            settings.OAuth2RedirectUrl($"{url}/swagger/oauth2-redirect.html");
            settings.DisplayRequestDuration();
        });
    }
}