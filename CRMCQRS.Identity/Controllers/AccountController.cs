using CRMCQRS.Application.Dto.Users;
using CRMCQRS.Domain;
using CRMCQRS.Identity.Application.Services;
using CRMCQRS.Identity.Endpoints.Account.Queries;
using CRMCQRS.Identity.Endpoints.Account.ViewModel;
using CRMCQRS.Infrastructure.Authentication.Policies.Schemes;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRMCQRS.Identity.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
public class AccountController : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("register")]
    public async Task<ActionResult<UserAccountViewModel>> Registration([FromBody] RegisterViewModel request)
    {
        var result = await Mediator.Send(new RegisterAccountRequest(request), HttpContext.RequestAborted);
        return Ok(result);
    }

    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [HttpGet]
    [Route("getuserbyid")]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes, Policy = "Account:GetAccountById")]
    public async Task<User> GetAccountById(
        [FromQuery] string id,
        [FromServices] IMediator mediator)
        => await mediator.Send(new GetAccountByIdRequest(id));

    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [HttpGet]
    [Route("getcurrentaccountclaims")]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes)]
    public async Task<IEnumerable<ClaimsViewModel>> GetClaims(
        [FromServices] IMediator mediator)
        => await mediator.Send(new GetClaimsRequest(), HttpContext.RequestAborted);

    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [HttpGet]
    [Route("getcurrentaccount")]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes)]
    public async Task<User> GetCurrentAccount([FromServices] IAccountService accountService)
        => await accountService.GetCurrentUserAsync();

    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [HttpGet]
    [Route("getcurrentaccountid")]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes)]
    public async Task<Guid> GetCurrentAccountId([FromServices] IAccountService accountService)
        => accountService.GetCurrentUserId();


    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [HttpPost]
    [Route("singin")]
    public async Task<IActionResult> Login([FromBody] LoginDto request,
        [FromServices] UserManager<User> userManager,
        [FromServices] SignInManager<User> signInManager,
        [FromServices] IAccountService accountService)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return NotFound();
        }

        await signInManager.SignInAsync(user, true);

        var signInResult = await signInManager.PasswordSignInAsync(user, request.Password, true, false);
        if (signInResult.Succeeded)
        {
            var principal = await accountService.GetPrincipalByIdAsync(user.Id.ToString());
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Ok();
        }
        return Forbid();
    }
}