using Application.DTOs;
using Application.UseCases.Queries.UserQueries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.QueryHandlers.UserQueryHandlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
            
            // Add roles to the DTO
            var identityUser = await _userManager.FindByIdAsync(request.Id.ToString());
            if (identityUser != null)
            {
                var roles = await _userManager.GetRolesAsync(identityUser);
                userDto.Roles = roles.ToList();
            }
            
            return Result<UserDto>.Success(userDto);
        }
    }
}