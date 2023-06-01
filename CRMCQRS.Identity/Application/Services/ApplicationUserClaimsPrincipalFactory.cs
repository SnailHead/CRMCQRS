using System.Collections.Immutable;
using System.Security.Claims;
using CRMCQRS.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace CRMCQRS.Identity.Application.Services;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
{
    private UserManager<User> _userManager;

    public ApplicationUserClaimsPrincipalFactory(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
        _userManager = userManager;
    }

    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var principal = await base.CreateAsync(user);
        var claimIdentity = (ClaimsIdentity)principal.Identity!;

        if (user.Permissions != null)
        {
            var permissions = user.Permissions.ToList();
            if (permissions.Any())
            {
                permissions.ForEach(x =>
                    ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim(x.PolicyName,
                        nameof(x.PolicyName).ToLower())));
            }
        }

        if (!string.IsNullOrWhiteSpace(user.UserName) && 
            string.IsNullOrEmpty(ClaimsHelper.GetValue<string>(claimIdentity,OpenIddictConstants.Claims.Name)))
        {
            ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim(OpenIddictConstants.Claims.Name, user.UserName));
        }

        if (!string.IsNullOrWhiteSpace(user.Firstname) &&
            string.IsNullOrEmpty(ClaimsHelper.GetValue<string>(claimIdentity,OpenIddictConstants.Claims.GivenName)))
        {
            ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim(OpenIddictConstants.Claims.GivenName, user.Firstname));
        }

        if (!string.IsNullOrWhiteSpace(user.Lastname) &&
            string.IsNullOrEmpty(ClaimsHelper.GetValue<string>(claimIdentity,OpenIddictConstants.Claims.FamilyName)))
        {
            ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim(OpenIddictConstants.Claims.FamilyName, user.Lastname));
        }
        if (!string.IsNullOrWhiteSpace(user.Email) &&
            string.IsNullOrEmpty(ClaimsHelper.GetValue<string>(claimIdentity,OpenIddictConstants.Claims.Email)))
        {
            ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim(OpenIddictConstants.Claims.Email, user.Email));
        }
        if (!string.IsNullOrWhiteSpace(user.PhoneNumber) &&
            string.IsNullOrEmpty(ClaimsHelper.GetValue<string>(claimIdentity,OpenIddictConstants.Claims.PhoneNumber)))
        {
            ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim(OpenIddictConstants.Claims.PhoneNumber, user.PhoneNumber));
        }
        if(!string.IsNullOrWhiteSpace(user.Id.ToString()) &&
           string.IsNullOrEmpty(ClaimsHelper.GetValue<string>(claimIdentity,OpenIddictConstants.Claims.Subject)))
        {
            ((ClaimsIdentity)principal.Identity!).AddClaim(new Claim(OpenIddictConstants.Claims.Subject, user.Id.ToString()));
        }

        if (string.IsNullOrWhiteSpace(ClaimsHelper.GetValue<string>(claimIdentity, OpenIddictConstants.Claims.Role)))
        {
            principal.SetClaims(OpenIddictConstants.Claims.Role, (await _userManager.GetRolesAsync(user)).ToImmutableArray());
        }
        
        return principal;
    }

}