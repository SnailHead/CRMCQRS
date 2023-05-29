using AutoMapper;
using CRMCQRS.API.Models.Users;
using CRMCQRS.Application.Users.Commands.CreateUser;
using CRMCQRS.Application.Users.Commands.DeleteUser;
using CRMCQRS.Application.Users.Commands.UpdateUser;
using CRMCQRS.Application.Users.Queries;
using CRMCQRS.Application.Users.Queries.GetPageUser;
using CRMCQRS.Application.Users.Queries.GetUser;
using CRMCQRS.Infrastructure.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMCQRS.API.Controllers;
[Produces("application/json")]
[Route("api/[controller]")]
public class UsersController : BaseController
{
    private readonly IMapper _mapper;

    public UsersController(IMapper mapper) => _mapper = mapper;

    [HttpPost]
    [Route("GetAll")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IPagedList<UserViewModel>>> GetAll([FromBody] GetPageUserDto request)
    {

        var query = _mapper.Map<GetPageUserQuery>(request);
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserViewModel>> Get(Guid id)
    {
        var query = new GetUserQuery { Id = id };
        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    [HttpPost]
    [Route("Create")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateUserDto request)
    {
        var command = _mapper.Map<CreateUserCommand>(request);
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }

    [HttpGet]
    [Route("Update")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> Update([FromBody] UpdateUserDto request)
    {
        var command = _mapper.Map<UpdateUserCommand>(request);
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }

    [HttpGet]
    [Route("Delete")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var command = new DeleteUserCommand() { Id = id };
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }
}