using Application.Services.Core.Repositories;
using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories.Core;

public class UserEmailAuthenticatorRepository : EfRepositoryBase<UserEmailAuthenticator, BaseDbContext>, IUserEmailAuthenticatorRepository
{
    public UserEmailAuthenticatorRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<ICollection<UserEmailAuthenticator>> DeleteAllNonVerifiedAsync(User user)
    {
        List<UserEmailAuthenticator> userEmailAuthenticators = Query()
            .Where(x => x.UserId == user.Id && x.IsVerified == false)
            .ToList();
        foreach (UserEmailAuthenticator userEmailAuthenticator in userEmailAuthenticators)
        {
            Context.Entry(userEmailAuthenticator).State = EntityState.Deleted;
        }
        await Context.SaveChangesAsync();
        return userEmailAuthenticators;
    }
}
