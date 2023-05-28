﻿using AutoMapper;
using CRMCQRS.API.Models.Projects;
using CRMCQRS.Application.Projects.Commands.CreateProject;
using CRMCQRS.Application.Projects.Commands.DeleteProject;
using CRMCQRS.Application.Projects.Commands.UpdateProject;
using CRMCQRS.Application.Projects.Queries;
using CRMCQRS.Application.Projects.Queries.GetPageProject;
using CRMCQRS.Application.Projects.Queries.GetProject;
using CRMCQRS.Infrastructure.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMCQRS.API.Controllers;

public class ProjectsController : BaseController
{
    private readonly IMapper _mapper;

    public ProjectsController(IMapper mapper) => _mapper = mapper;

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IPagedList<ProjectViewModel>>> GetAll([FromBody] GetPageProjectDto request)
    {

        var query = _mapper.Map<GetPageProjectQuery>(request);
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProjectViewModel>> Get(Guid id)
    {
        var query = new GetProjectQuery { Id = id };
        var vm = await Mediator.Send(query);

        return Ok(vm);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateProjectDto request)
    {
        var command = _mapper.Map<CreateProjectCommand>(request);
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> Update([FromBody] UpdateProjectDto request)
    {
        var command = _mapper.Map<UpdateProjectCommand>(request);
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var command = new DeleteProjectCommand() { Id = id };
        var vm = await Mediator.Send(command);
        return Ok(vm);
    }
}