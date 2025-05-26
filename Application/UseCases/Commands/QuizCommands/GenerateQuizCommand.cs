using Domain.Common;
using MediatR;

namespace Application.UseCases.Commands.QuizCommands;

public class GenerateQuizCommand : IRequest<Result<Guid>>
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public int NumberOfQuestions { get; set; }
} 