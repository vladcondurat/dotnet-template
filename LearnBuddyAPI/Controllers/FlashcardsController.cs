using Application.DTOs;
using Application.UseCases.Commands.FlashcardCommands;
using Application.UseCases.Queries.FlashcardQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Extensions;

namespace WebApplication.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class FlashcardsController : ApplicationController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public FlashcardsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateFlashcards([FromBody] GenerateFlashcardsRequest request)
    {
        ValidateUserId();
        var command = _mapper.Map<GenerateFlashcardsCommand>(request);
        command.UserId = UserId!.Value;
        var result = await _mediator.Send(command);
        return result.To201ActionResult();
    }
    
    [HttpPost("toggle-known/{flashcardId:guid}")]
    public async Task<IActionResult> ToggleFlashcardKnown(Guid flashcardId)
    {
        ValidateUserId();
        var command = new ToggleFlashcardKnownCommand { FlashcardId = flashcardId, UserId = UserId!.Value };
        var result = await _mediator.Send(command);
        return result.To204ActionResult();
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetAllFlashcardsByProjectId(Guid projectId)
    {
        ValidateUserId();
        var query = new GetAllFlashcardsByProjectIdQuery { ProjectId = projectId, UserId = UserId!.Value};
        var result = await _mediator.Send(query);
        return result.To200ActionResult();
    }
} 