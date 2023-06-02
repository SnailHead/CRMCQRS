using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace CRMCQRS.Infrastructure.Database
{
    public class DatabaseInitializer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DefaultDbContext _context;

        public DatabaseInitializer(IServiceProvider serviceProvider, DefaultDbContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }

        public async Task Seed()
        {
            await _context!.Database.EnsureCreatedAsync();
            var pending =
                await _context.Database
                    .GetPendingMigrationsAsync(); //Асинхронно получает все миграции, определенные в сборке, но не примененные к целевой базе данных.
            if (pending.Any())
            {
                await _context!.Database.MigrateAsync();
            }

            await SeedRoles();
            await SeedPermissions();
            await SeedUsers();
            await SeedClients();
        }

        private async Task SeedClients()
        {
            using var scope = _serviceProvider.CreateScope();

            var applicationManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var applications = await applicationManager.ListAsync().ToListAsync();
            if (!applications.Exists(item => (item as OpenIddictApplication).ClientId == "CRMCQRS-Identity-ID"))
            {
                await applicationManager.CreateAsync(new OpenIddictApplicationDescriptor()
                {
                    ClientId = "CRMCQRS-Identity-ID",
                    ClientSecret = "CRMCQRS-Identity-SECRET",
                    DisplayName = "CRMCQRS-Identity",
                    ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.Logout,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.GrantTypes.Implicit,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                        OpenIddictConstants.Permissions.ResponseTypes.Code, 
                        OpenIddictConstants.Permissions.ResponseTypes.Token
                    },
                    Type = OpenIddictConstants.ClientTypes.Confidential,
                    RedirectUris = { new Uri("https://localhost:5001") }
                });
            }
        }

        private async Task SeedUsers()
        {
            using var scope = _serviceProvider.CreateScope();

            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var adminFromConfig = configuration.GetSection("AdminUser").Get<AdminProfileOption>();
            if (adminFromConfig is null)
            {
                throw new ArgumentNullException("adminFromConfig", "Admin Profile не найден в appsetting.json");
            }

            var currentAdmin = await _context.Users
                .Include(x => x.Permissions)
                .FirstOrDefaultAsync(x => x.UserName == adminFromConfig.UserName && x.Email == adminFromConfig.Email);

            if (currentAdmin is not null)
            {
                foreach (var permission in _context.Permissions.ToList())
                {
                    if (currentAdmin.Permissions.Any(x => x.Id == permission.Id))
                    {
                        continue;
                    }

                    currentAdmin.Permissions.Add(permission);
                }

                await _context.SaveChangesAsync();
                return;
            }

            var admin = new User
            {
                Email = adminFromConfig.Email,
                UserName = adminFromConfig.UserName,
                Firstname = adminFromConfig.FirstName,
                Lastname = adminFromConfig.LastName,
                PhoneNumber = adminFromConfig.PhoneNumber,
                EmailConfirmed = adminFromConfig.EmailConfirmed,
                PhoneNumberConfirmed = adminFromConfig.PhoneNumberConfirmed,
                BirthDate = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            if (!_context!.Users.Any(u => u.UserName == admin.UserName))
            {
                var password = new PasswordHasher<User>();
                admin.PasswordHash = password.HashPassword(admin, adminFromConfig.Password);

                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var userResult = await userManager.CreateAsync(admin);
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Cannot create account {userResult.Errors.FirstOrDefault()?.Description}");
                }

                var roleResult = await userManager.AddToRoleAsync(admin, Roles.Roles.Admin);
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Cannot add roles to account");
                }

                await _context.SaveChangesAsync();

                var permissions = await _context.Permissions.ToListAsync();
                var user = await _context.Users.Where(x => x.Id == admin.Id).FirstOrDefaultAsync();

                user.Permissions = permissions;
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedRoles()
        {
            using var scope = _serviceProvider.CreateScope();

            var roles = Roles.Roles.RoleNames;
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            foreach (var role in roles)
            {
                bool check = await roleManager.RoleExistsAsync(role);
                if (!check)
                {
                    await roleManager.CreateAsync(new Role(role));
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedPermissions()
        {
            using var scope = _serviceProvider.CreateScope();
            var identityConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("identitysetting.json")
                .Build();
            var options = identityConfiguration.GetSection("Permissions").Get<List<IdentityPermissionOption>>();

            foreach (var option in options)
            {
                var permission = _context.Permissions.Where(x => x.PolicyName == option.Name).ToList();
                if (permission.Any())
                {
                    continue;
                }

                await _context.Permissions.AddAsync(new Permission()
                    { PolicyName = option.Name, Description = option.Description });
            }

            await _context.SaveChangesAsync();
        }
    }
}