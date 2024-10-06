using Auth.Data.Context;
using Auth.Data.Entities;
using Auth.Data.Repository.Interfaces;

namespace Auth.Data.Repository
{
    public class UserPasswordHistoryRepository : GenericRepository<UserPasswordHistory>, IUserPasswordHistoryRepository
    {
        private readonly AuthDbContext _dbContext;

        public UserPasswordHistoryRepository(AuthDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> GetLastThreePasswordByUserId(int userId)
        {
            var passwordList = _dbContext.UserPasswordHistory.Where(x => x.UserId == userId)
                                                             .OrderByDescending(x => x.CreatedDate)
                                                             .Select(x => x.Password)
                                                             .Take(3)
                                                             .ToList();

            return passwordList;
        }
    }
}