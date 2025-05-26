using Application.DTOs;
using Application.UseCases.Commands.ProjectCommands;
using Application.UseCases.Queries.ProjectQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Extensions;
using WebApplication.Filters;

namespace WebApplication.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class ProjectsController : ApplicationController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProjectsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllProjects()
    {
        ValidateUserId();
        var result = await _mediator.Send(new GetAllProjectsQuery{ UserId = UserId!.Value });
        return result.To200ActionResult();
    }
    
    [HttpGet("{id:guid}")]
    [ValidateIdMatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProjectSummary([FromRoute] Guid id)
    {
        ValidateUserId();
        var result = await _mediator.Send(new GetProjectSummaryQuery{ UserId = UserId!.Value, ProjectId = id });
        return result.To200ActionResult();
    }

    [HttpPatch("{id:guid}")]
    [ValidateIdMatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProject([FromRoute] Guid id, [FromBody] UpdateProjectRequest request)
    {
        ValidateUserId();
        var command = _mapper.Map<UpdateProjectCommand>(request);
        command.UserId = UserId!.Value;
        command.Id = id;
        var result = await _mediator.Send(command);
        return result.To204ActionResult();
    }
    
    [HttpPatch("summary/{id:guid}")]
    [ValidateIdMatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProjectSummary([FromRoute] Guid id, [FromBody] UpdateProjectSummaryRequest request)
    {
        ValidateUserId();
        var command = _mapper.Map<UpdateProjectSummaryCommand>(request);
        command.UserId = UserId!.Value;
        command.Id = id;
        var result = await _mediator.Send(command);
        return result.To204ActionResult();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        ValidateUserId();
        var command = new DeleteProjectCommand { Id = id, UserId = UserId!.Value };
        var result = await _mediator.Send(command);
        return result.To204ActionResult();
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        ValidateUserId();
        var command = _mapper.Map<CreateProjectCommand>(request);
        command.UserId = UserId!.Value;
        var result = await _mediator.Send(command);
        return result.To201ActionResult();
    }
} 