using Auth.Data.Context;
using Auth.Data.Entities;
using Auth.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AuthDbContext _dbContext;

        public UserRepository(AuthDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByEmailWithRoles(string email)
        {
            var user = await _dbContext.Users.Where(x => x.Email == email)
                                       .Include(x => x.UserRoles)
                                            .ThenInclude(x => x.Role)
                                       .FirstOrDefaultAsync();

            return user;
        }
    }
}
