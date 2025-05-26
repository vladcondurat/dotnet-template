using Application.UseCases.Commands.UserCommands;
using Domain.Common;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.UserCommandHandlers
{
    public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, Result>
    {   
        private readonly IUnitOfWork _unitOfWork;
        
        public DeleteUserByIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.FindAsync(request.Id);
            if (user == null)
            {
                return Error.EntityNotFound(request.Id, typeof(Domain.Entities.User));
            }

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}