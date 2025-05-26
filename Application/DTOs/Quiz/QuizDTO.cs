namespace Application.DTOs;

public class QuizDto
{
    public Guid QuizId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TimeSpan Timer { get; set; }
    public int NumberOfQuestions { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
} 