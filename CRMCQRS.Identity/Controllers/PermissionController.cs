using CRMCQRS.Domain;
using CRMCQRS.Identity.Endpoints.Permission.Queries;
using CRMCQRS.Identity.Endpoints.Permission.ViewModel;
using CRMCQRS.Infrastructure.Authentication.Policies.Schemes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMCQRS.Identity.Controllers;

public class PermissionController : BaseController
{
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [HttpGet]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes, Policy = "Permission:Get")]
    public static async Task<IList<CRMCQRS.Domain.Permission>> GetAll(
        [FromServices] IMediator mediator)
        => await mediator.Send(new GetAllRequest());

    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [HttpGet]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes, Policy = "Permission:Get")]
    public static async Task<CRMCQRS.Domain.Permission> GetById(
        [FromQuery] Guid id,
        [FromServices] IMediator mediator)
    => await mediator.Send(new GetByIdRequest(id));
    
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes, Policy = "Permission:Change")]
    public static async Task<Permission> Insert(
        [FromBody] PermissionViewModel model,
        [FromServices] IMediator mediator)
    => await mediator.Send(new InsertRequest(model));

    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes, Policy = "Permission:Change")]
    public static async Task<CRMCQRS.Domain.Permission> Update(
        [FromQuery] Guid id,
        [FromBody] PermissionViewModel model,
        [FromServices] IMediator mediator)
        => await mediator.Send(new UpdateRequest(id, model));

    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes, Policy = "Permission:Profile")]
    public static async Task<ProfilePermissionViewModel> AddToProfile(
        [FromBody] ProfilePermissionViewModel model,
        [FromServices] IMediator mediator)
        => await mediator.Send(new AddToProfileRequest(model));

    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(AuthenticationSchemes = AuthSchemes.AuthenticationSchemes, Policy = "Permission:Profile")]
    public static async Task<ProfilePermissionViewModel> RemoveInProfile(
        [FromBody] ProfilePermissionViewModel model,
        [FromServices] IMediator mediator)
        => await mediator.Send(new RemoveInProfileRequest(model));
}