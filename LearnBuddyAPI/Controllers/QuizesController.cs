using Application.DTOs;
using Application.UseCases.Commands.QuizCommands;
using Application.UseCases.Queries.QuizQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Extensions;

namespace WebApplication.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class QuizzesController : ApplicationController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public QuizzesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("generate")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GenerateQuiz([FromBody] GenerateQuizRequest request)
    {
        ValidateUserId();
        var command = _mapper.Map<GenerateQuizCommand>(request);
        command.UserId = UserId!.Value;
        var result = await _mediator.Send(command);
        return result.To201ActionResult();
    }

    [HttpGet("{quizId:guid}")]
    [ProducesResponseType(typeof(QuizDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetQuizById(Guid quizId)
    {
        ValidateUserId();
        var query = new GetQuizByIdQuery { QuizId = quizId, UserId = UserId!.Value };
        var result = await _mediator.Send(query);
        return result.To200ActionResult();
    }

    [HttpGet("project/{projectId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<QuizDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllQuizzesByProjectId(Guid projectId)
    {
        ValidateUserId();
        var query = new GetAllQuizzesByProjectIdQuery { ProjectId = projectId, UserId = UserId!.Value };
        var result = await _mediator.Send(query);
        return result.To200ActionResult();        
    }
} 