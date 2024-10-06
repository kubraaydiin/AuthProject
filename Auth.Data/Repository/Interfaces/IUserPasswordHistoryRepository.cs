using Auth.Data.Entities;

namespace Auth.Data.Repository.Interfaces
{
    public interface IUserPasswordHistoryRepository : IGenericRepository<UserPasswordHistory>
    {
        Task<List<string>> GetLastThreePasswordByUserId(int userId);
    }
}
