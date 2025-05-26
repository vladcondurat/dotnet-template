namespace Application.DTOs;

public class QuizQuestionDto
{
    public Guid Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public IEnumerable<string> IncorrectAnswers { get; set; } = new List<string>();
} 