using Application.Services.Core.Repositories;
using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.Core;

public class UserRepository : EfRepositoryBase<User, BaseDbContext>, IUserRepository
{
    public UserRepository(BaseDbContext context) : base(context)
    {
    }
}