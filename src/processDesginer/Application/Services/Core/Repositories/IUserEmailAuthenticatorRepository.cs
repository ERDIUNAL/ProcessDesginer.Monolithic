using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;

namespace Application.Services.Core.Repositories;

public interface IUserEmailAuthenticatorRepository : IRepository<UserEmailAuthenticator>, IAsyncRepository<UserEmailAuthenticator>
{
    public Task<ICollection<UserEmailAuthenticator>> DeleteAllNonVerifiedAsync(User user);
}
