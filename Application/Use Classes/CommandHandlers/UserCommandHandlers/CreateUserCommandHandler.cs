using Application.Use_Classes.Commands.UserCommands;
using AutoMapper;
using Domain.Common;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Classes.CommandHandlers.UserCommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                return Error.ValidationError("Entity with the same username already exists.");
            }

            var user = _mapper.Map<Domain.Entities.User>(request);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            
            return Result<Guid>.Success(user.Id);
        }
    }
}