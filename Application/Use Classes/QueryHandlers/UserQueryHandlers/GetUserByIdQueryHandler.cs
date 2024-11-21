using Application.DTOs;
using Application.Use_Classes.Queries.UserQueries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Classes.QueryHandlers.UserQueryHandlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userResult = await _unitOfWork.Users.FindAsync(request.Id);
            
            if (userResult == null)
            {
                return Error.EntityNotFound(request.Id, typeof(User));
            }
            
            var userDto = _mapper.Map<UserDto>(userResult);
            return Result<UserDto>.Success(userDto);
        }
    }
}