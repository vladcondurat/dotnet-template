namespace Domain.Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync();
    int SaveChanges();
    
    Task ReloadAsync<T>(T entity) where T : class;
    void Reload<T>(T entity) where T : class;
    
    bool IsModified<T>(T entity) where T : class;
    
    void Detach<T>(T entity) where T : class;
}