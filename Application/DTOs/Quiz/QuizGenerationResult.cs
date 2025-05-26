namespace Application.DTOs;

public class QuestionGenerationResult
{
    public string Question { get; set; } = string.Empty;
    public IEnumerable<string> IncorrectAnswers { get; set; } = new List<string>();
    public string CorrectAnswer { get; set; } = string.Empty;
}