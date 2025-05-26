using Application.UseCases.Commands.ProjectCommands;
using Domain.Common;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.ProjectCommandHandlers;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.FindAsync(request.Id);
        if (project == null)
        {
            return Error.EntityNotFound(request.Id, typeof(Domain.Entities.Project));
        }
        
        if (project.UserId != request.UserId)
        {
            return Error.Forbidden("You do not have permission to delete this project.");
        }

        _unitOfWork.Projects.Delete(project);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
} 