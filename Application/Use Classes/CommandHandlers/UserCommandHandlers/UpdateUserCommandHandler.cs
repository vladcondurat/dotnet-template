using Application.Use_Classes.Commands.UserCommands;
using AutoMapper;
using Domain.Common;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Classes.CommandHandlers.UserCommandHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.FindAsync(request.Id);
            if (user == null)
            {
                return Error.EntityNotFound(request.Id, typeof(Domain.Entities.User));
            }

            _mapper.Map(request, user);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}