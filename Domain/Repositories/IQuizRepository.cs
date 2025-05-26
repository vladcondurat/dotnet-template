 using Domain.Entities;

namespace Domain.Repositories
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        Task<Quiz?> GetQuizWithQuestionsAndProjectAsync(Guid quizId);
        Task<IEnumerable<Quiz>> GetQuizzesByProjectIdAsync(Guid projectId);
    }
} 