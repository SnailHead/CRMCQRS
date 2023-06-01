using System.Reflection;
using CRMCQRS.Domain;
using CRMCQRS.Identity.Application.Services;
using CRMCQRS.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace CRMCQRS.Identity.BuilderExtensions
{
    public static class BuilderDatabase
    {
        public static void ConfigureDatabaseServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            string migrationsAssembly = typeof(DefaultDbContext).GetTypeInfo().Assembly.GetName().Name!;
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=localhost;Initial Catalog=CRMCQRS;Integrated Security=True;TrustServerCertificate=True";

            services.AddDbContext<DefaultDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(migrationsAssembly));
                options.UseOpenIddict<OpenIddictApplication, OpenIddictAuthorization, OpenIddictScope, OpenIddictToken, Guid>();
            });
            
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
                options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email;
            });
            
            services.AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireUppercase = false;
                })
                .AddSignInManager()
                .AddEntityFrameworkStores<DefaultDbContext>()
                .AddUserManager<UserManager<User>>()
                .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
                .AddDefaultTokenProviders();
        }
        public static async Task ConfigureDatabaseApplication(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<DefaultDbContext>()!;

            await new DatabaseInitializer(app.Services, context).Seed();
        }
    }
    
}
