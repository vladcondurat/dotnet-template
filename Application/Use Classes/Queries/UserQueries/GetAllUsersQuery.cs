using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Use_Classes.Queries.UserQueries;

public class GetAllUsersQuery : IRequest<Result<IEnumerable<UserDto>>>
{
    
}