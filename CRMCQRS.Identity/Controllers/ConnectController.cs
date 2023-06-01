using CRMCQRS.Domain;
using CRMCQRS.Identity.Application.Dto;
using CRMCQRS.Identity.Application.Services;
using CRMCQRS.Identity.Helper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace CRMCQRS.Identity.Controllers;

[Produces("application/json")]
[Route("api/connect")]
public class ConnectController : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes("application/x-www-form-urlencoded")]
    [Route("token")]
    public async Task<IResult> Token(
        [FromForm] TokenDto request,
        [FromServices] IOpenIddictScopeManager manager,
        [FromServices] UserManager<User> userManager,
        [FromServices] SignInManager<User> signInManager,
        [FromServices] IAccountService accountService)
    {
        var openIdRequest = HttpContext.GetOpenIddictServerRequest() ??
                            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
        if (openIdRequest.IsClientCredentialsGrantType())
        {
            return await ConnectHelper.ConnectClientCredentialsGrantType(openIdRequest, manager);
        }

        if (openIdRequest.IsPasswordGrantType())
        {
            return await ConnectHelper.ConnectPasswordGrantType(HttpContext, openIdRequest, userManager, signInManager, manager,
                accountService);
        }

        if (openIdRequest.IsAuthorizationCodeGrantType())
        {
            return await ConnectHelper.ConnectAuthorizationCodeGrantType(HttpContext);
        }

        if (openIdRequest.IsDeviceCodeGrantType())
        {
            return await ConnectHelper.ConnectDeviceCodeGrantType(HttpContext, userManager, signInManager);
        }

        if (openIdRequest.IsRefreshTokenGrantType())
        {
            return await ConnectHelper.ConnectRefreshTokenGrantType(openIdRequest, HttpContext, accountService, manager,
                userManager, signInManager);
        }

        return Results.Problem("The specified grant type is not supported.");
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Consumes("application/x-www-form-urlencoded")]
    [Route("authorize")]
    public async Task<IResult> Authorize(
        [FromServices] IOpenIddictScopeManager scopeManager,
        [FromServices] UserManager<User> userManager,
        [FromServices] SignInManager<User> signInManager,
        [FromServices] IOpenIddictApplicationManager applicationManager,
        [FromServices] IOpenIddictAuthorizationManager authorizationManager)
    {
        var iddictRequest = HttpContext.GetOpenIddictServerRequest() ??
                            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // Retrieve the user principal stored in the authentication cookie.
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded)
        {
            return Results.Challenge(new AuthenticationProperties
                {
                    RedirectUri = HttpContext.Request.PathBase + HttpContext.Request.Path + QueryString.Create(HttpContext.Request.HasFormContentType
                        ? HttpContext.Request.Form.ToList()
                        : HttpContext.Request.Query.ToList())
                },
                new List<string> { CookieAuthenticationDefaults.AuthenticationScheme });
        }

        var user = await userManager.GetUserAsync(result.Principal) ??
                   throw new InvalidOperationException("The user details cannot be retrieved.");

        var application = await applicationManager.FindByClientIdAsync(iddictRequest.ClientId!) ??
                          throw new InvalidOperationException(
                              "Details concerning the calling client application cannot be found.");
        var applicationId = await applicationManager.GetIdAsync(application);
        var userId = await userManager.GetUserIdAsync(user);

        var authorizations = await authorizationManager.FindAsync(
            subject: userId,
            client: applicationId!,
            status: OpenIddictConstants.Statuses.Valid,
            type: OpenIddictConstants.AuthorizationTypes.Permanent,
            scopes: iddictRequest.GetScopes()).ToListAsync();

        return await ConnectHelper.Authorize(
            applicationManager, scopeManager, authorizationManager,
            signInManager, userManager, application, authorizations,
            user, applicationId, iddictRequest);
    }
}