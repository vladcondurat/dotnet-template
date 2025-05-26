namespace Domain.Entities;

public class QuizQuestion
{
    public Guid Id { get; set; }
    public string Question { get; set; } = null!;
    public string CorrectAnswer { get; set; } = null!;
    public IEnumerable<string> IncorrectAnswers { get; set; } = new List<string>();

    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    
}