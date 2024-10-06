using Auth.Data.Entities;

namespace Auth.Data.Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailWithRoles(string email);
    }
}
