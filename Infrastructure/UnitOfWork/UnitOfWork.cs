using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dBContext;

        public UnitOfWork(
            ApplicationDbContext dBContext,
            IUserRepository userRepository
            )
        {
            _dBContext = dBContext;
            Users = userRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dBContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _dBContext.SaveChanges();
        }

        public async Task ReloadAsync<T>(T entity) where T : class
        {
            await _dBContext.Entry(entity).ReloadAsync();
        }

        public void Reload<T>(T entity) where T : class
        {
            _dBContext.Entry(entity).Reload();
        }

        public bool IsModified<T>(T entity) where T : class
        {
            return _dBContext.Entry(entity).State == EntityState.Modified;
        }

        public void Detach<T>(T entity) where T : class
        {
            _dBContext.Entry(entity).State = EntityState.Detached;
        }

        #region Repositories

        public IUserRepository Users { get; }

        #endregion
    }
}
