using Auth.Data.Context;
using Auth.Data.Entities;
using Auth.Data.Repository.Interfaces;

namespace Auth.Data.Repository
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AuthDbContext dbContext) : base(dbContext)
        {
        }
    }
}
