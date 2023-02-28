using Application.Services.Core.Repositories;
using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.Core;

public class UserOtpAuthenticatorRepository : EfRepositoryBase<UserOtpAuthenticator, BaseDbContext>, IUserOtpAuthenticatorRepository
{
    public UserOtpAuthenticatorRepository(BaseDbContext context) : base(context)
    {

    }
}
