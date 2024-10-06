using Auth.Data.Repository.Interfaces;

namespace Auth.Data.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IUserPasswordHistoryRepository UserPasswordHistoryRepository { get; }

        Task<int> SaveChanges();
    }
}
