using Application.DTOs;

namespace Application.Services;

public interface IGeminiService
{
    Task<string> GenerateSummaryAsync(string content, int size);

    Task<IEnumerable<QuestionGenerationResult>> GenerateQuestionsAsync(
        string topic,
        int numberOfQuestions,
        string difficulty,
        string summaryText);

    Task<IEnumerable<FlashcardGenerationResult>> GenerateFlashcardsAsync(
        string projectSummary,
        int numberOfFlashcards,
        string difficulty,
        string? focusArea);
}