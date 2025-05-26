namespace Domain.Entities;

public class Quiz
{
    public Guid QuizId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TimeSpan Timer { get; set; }
    public int NumberOfQuestions { get; set; }
    public string Difficulty { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public IEnumerable<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
}