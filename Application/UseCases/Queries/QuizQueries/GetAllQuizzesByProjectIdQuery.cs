using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.QuizQueries;

public class GetAllQuizzesByProjectIdQuery : IRequest<Result<IEnumerable<QuizDto>>>
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
} 