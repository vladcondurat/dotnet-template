using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.UserQueries;

public class GetCurrentUserQuery : IRequest<Result<UserDto>>
{
    public Guid UserId { get; set; }
} 