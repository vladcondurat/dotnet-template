using Application.Services;
using Application.UseCases.Commands.ProjectCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.ProjectCommandHandlers;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeminiService _geminiService;

    public CreateProjectCommandHandler(IUnitOfWork unitOfWork, IGeminiService geminiService)
    {
        _unitOfWork = unitOfWork;
        _geminiService = geminiService;
    }

    public async Task<Result<Guid>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var summary = await _geminiService.GenerateSummaryAsync(request.Content, request.Size);
        var project = new Project
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UserId = request.UserId,
            Summary = summary
        };
        
        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<Guid>.Success(project.Id);
    }
} 