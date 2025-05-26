using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FlashcardRepository : Repository<Flashcard>, IFlashcardRepository
{
    public FlashcardRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Flashcard>> GetFlashcardsByProjectIdAsync(Guid projectId)
    {
        return await DbContext.Flashcards
            .Where(f => f.ProjectId == projectId)
            .ToListAsync();
    }
    
    public async Task<Flashcard?> GetFlashcardByIdIncludeProjectAsync(Guid flashcardId)
    {
        return await DbContext.Flashcards
            .Include(f => f.Project)
            .FirstOrDefaultAsync(f => f.FlashcardId == flashcardId);
    }
} 