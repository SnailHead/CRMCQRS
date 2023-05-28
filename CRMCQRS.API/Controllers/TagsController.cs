using AutoMapper;
using CRMCQRS.API.Models;
using CRMCQRS.API.Models.Tags;
using CRMCQRS.Application.Tags.Commands.CreateTag;
using CRMCQRS.Application.Tags.Commands.DeleteTag;
using CRMCQRS.Application.Tags.Commands.UpdateTag;
using CRMCQRS.Application.Tags.Queries;
using CRMCQRS.Application.Tags.Queries.GetPageTag;
using CRMCQRS.Application.Tags.Queries.GetTag;
using CRMCQRS.Infrastructure.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMCQRS.API.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
public class TagsController : BaseController
{
    private readonly IMapper _mapper;

    public TagsController(IMapper mapper) => _mapper = mapper;
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IPagedList<TagViewModel>>> GetAll([FromBody] GetPageTagDto request)
    {

        var query = _mapper.Map<GetPageTagQuery>(request);
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TagViewModel>> Get(Guid id)
    {
        var query = new GetTagQuery { Id = id };
        var vm = await Mediator.Send(query);

        return Ok(vm);
    }
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateTagDto request)
    {
        var command =  _mapper.Map<CreateTagCommand>(request);
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> Update([FromBody] UpdateTagDto request)
    {
        var command = _mapper.Map<UpdateTagCommand>(request);
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var command = new DeleteTagCommand() { Id = id };
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }
}