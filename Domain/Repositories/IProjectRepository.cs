using Domain.Entities;

namespace Domain.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetProjectsByUserIdAsync(Guid userId);
}