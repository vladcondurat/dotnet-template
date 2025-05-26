using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.UseCases.Queries.QuizQueries;

public class GetQuizByIdQuery : IRequest<Result<QuizDetailDto>>
{
    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }
} 