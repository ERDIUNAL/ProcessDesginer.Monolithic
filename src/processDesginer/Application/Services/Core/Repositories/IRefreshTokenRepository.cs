using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;

namespace Application.Services.Core.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>, IAsyncRepository<RefreshToken>
{
    public Task<ICollection<RefreshToken>> GetAllOldActiveRefreshTokensAsync(User user, int ttl);
}
