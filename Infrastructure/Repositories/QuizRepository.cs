using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class QuizRepository : Repository<Quiz>, IQuizRepository
{
    public QuizRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Quiz?> GetQuizWithQuestionsAndProjectAsync(Guid quizId)
    {
        return await DbContext.Quizzes
            .Include(q => q.Questions)
            .Include(q => q.Project)
            .FirstOrDefaultAsync(q => q.QuizId == quizId);
    }

    public async Task<IEnumerable<Quiz>> GetQuizzesByProjectIdAsync(Guid projectId)
    {
        return await DbContext.Quizzes
            .Where(q => q.ProjectId == projectId)
            .ToListAsync();
    }
}
