using Application.UseCases.Commands.ProjectCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.ProjectCommandHandlers;

public class UpdateProjectSummaryCommandHandler : IRequestHandler<UpdateProjectSummaryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProjectSummaryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateProjectSummaryCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.FindAsync(request.Id);
        if (project == null)
        {
            return Error.EntityNotFound(request.Id, typeof(Project));
        }
        
        if (project.UserId != request.UserId)
        {
            return Error.Forbidden("You do not have permission to update this project.");
        }

        _mapper.Map(request, project);
        _unitOfWork.Projects.Update(project);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}