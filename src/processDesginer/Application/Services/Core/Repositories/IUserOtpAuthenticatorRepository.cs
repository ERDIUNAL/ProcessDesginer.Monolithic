using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;

namespace Application.Services.Core.Repositories;

public interface IUserOtpAuthenticatorRepository : IRepository<UserOtpAuthenticator>, IAsyncRepository<UserOtpAuthenticator>
{
}
