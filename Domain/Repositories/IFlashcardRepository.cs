using Domain.Entities;

namespace Domain.Repositories
{
    public interface IFlashcardRepository : IRepository<Flashcard>
    {
        Task<IEnumerable<Flashcard>> GetFlashcardsByProjectIdAsync(Guid projectId);
        Task<Flashcard?> GetFlashcardByIdIncludeProjectAsync(Guid flashcardId);
    }
} 