using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.UserQueries;

public class GetUserByIdQuery : IRequest<Result<UserDto>>
{  
    public Guid Id { get; set; }
}