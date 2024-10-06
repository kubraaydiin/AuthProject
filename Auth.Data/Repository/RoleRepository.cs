using Auth.Data.Context;
using Auth.Data.Entities;
using Auth.Data.Repository.Interfaces;

namespace Auth.Data.Repository
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AuthDbContext dbContext) : base(dbContext)
        {
        }
    }
}
