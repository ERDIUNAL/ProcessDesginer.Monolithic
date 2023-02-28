using Application.Services.Core.Repositories;
using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories.Core;

public class RefreshTokenRepository : EfRepositoryBase<RefreshToken, BaseDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<ICollection<RefreshToken>> GetAllOldActiveRefreshTokensAsync(User user, int ttl)
    {
        return await Query().Where(x => x.UserId == user.Id &&
                                        x.RevokedDate == null &&
                                        x.ExpiresDate > DateTime.UtcNow &&
                                        x.CreatedDate.AddMinutes(ttl) < DateTime.UtcNow)
                            .ToListAsync();
    }
}
