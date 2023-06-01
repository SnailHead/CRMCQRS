using System.Security.Claims;
using CRMCQRS.Domain;
using CRMCQRS.Identity.Endpoints.Account.ViewModel;

namespace CRMCQRS.Identity.Application.Services;

public interface IAccountService
{
    Task<IEnumerable<User>> GetUsersByEmailsAsync(IEnumerable<string> emails);
    Guid GetCurrentUserId();

    Task<UserAccountViewModel> RegisterAsync(RegisterViewModel model, CancellationToken cancellationToken);
    Task<User> GetByIdAsync(Guid id);
    Task<ClaimsPrincipal> GetPrincipalByIdAsync(string identifier);
    Task<ClaimsPrincipal> GetPrincipalForUserAsync(User user);
    Task<User> GetCurrentUserAsync();
    Task<IEnumerable<User>> GetUsersInRoleAsync(string roleName);
}