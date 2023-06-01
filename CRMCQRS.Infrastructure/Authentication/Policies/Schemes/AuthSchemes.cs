using Microsoft.AspNetCore.Authentication.Cookies;
using OpenIddict.Validation.AspNetCore;

namespace CRMCQRS.Infrastructure.Authentication.Policies.Schemes;

public static class AuthSchemes
{
    public const string AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
}