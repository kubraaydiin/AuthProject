using Auth.Data.Context;
using Auth.Data.Repository;
using Auth.Data.Repository.Interfaces;

namespace Auth.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _dbContext;

        public UnitOfWork(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUserRepository UserRepository => new UserRepository(_dbContext);

        public IRoleRepository RoleRepository => new RoleRepository(_dbContext);

        public IUserRoleRepository UserRoleRepository => new UserRoleRepository(_dbContext);

        public IUserPasswordHistoryRepository UserPasswordHistoryRepository => new UserPasswordHistoryRepository(_dbContext);

        public async Task<int> SaveChanges() => await _dbContext.SaveChangesAsync();

        public void Dispose() => _dbContext.Dispose();
    }
}
