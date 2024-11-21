using Application.DTOs;
using Application.Use_Classes.Queries.UserQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Classes.QueryHandlers.UserQueryHandlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var usersResult = await _unitOfWork.Users.GetAllAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(usersResult);
            return Result<IEnumerable<UserDto>>.Success(userDtos);
        }
    }
}