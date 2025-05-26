using Application.Services;
using Application.UseCases.Commands.QuizCommands;
using Domain.Common;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.CommandHandlers.QuizCommandHandlers;

public class GenerateQuizCommandHandler : IRequestHandler<GenerateQuizCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeminiService _geminiService;

    public GenerateQuizCommandHandler(IUnitOfWork unitOfWork, IGeminiService geminiService)
    {
        _unitOfWork = unitOfWork;
        _geminiService = geminiService;
    }

    public async Task<Result<Guid>> Handle(GenerateQuizCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.FindAsync(request.ProjectId);
        if (project is null)
        {
            return Error.EntityNotFound(request.ProjectId, typeof(Project));
        }
        
        if (project.UserId != request.UserId)
        {
            return Error.Forbidden("You do not have permission to access this project.");
        }

        var quizId = Guid.NewGuid();
        var quiz = new Quiz
        {
            QuizId = quizId,
            Title = $"Quiz on {request.Topic}",
            Description = $"A quiz about {request.Topic} with {request.NumberOfQuestions} questions",
            Timer = TimeSpan.FromMinutes(request.NumberOfQuestions), // 1 minute per question
            NumberOfQuestions = request.NumberOfQuestions,
            Difficulty = request.Difficulty,
            CreatedAt = DateTime.UtcNow,
            ProjectId = request.ProjectId,
            Questions = new List<QuizQuestion>()
        };

        var generatedQuestions = await _geminiService.GenerateQuestionsAsync(
            request.Topic,
            request.NumberOfQuestions,
            request.Difficulty,
            project.Summary);

        quiz.Questions = generatedQuestions
            .Select(q => new QuizQuestion
            {
                Id = Guid.NewGuid(),
                QuizId = quizId,
                Question = q.Question,
                CorrectAnswer = q.CorrectAnswer,
                IncorrectAnswers = q.IncorrectAnswers
            })
            .ToList();

        await _unitOfWork.Quizzes.AddAsync(quiz);
        await _unitOfWork.SaveChangesAsync();

        return Result<Guid>.Success(quizId);
    }
}
