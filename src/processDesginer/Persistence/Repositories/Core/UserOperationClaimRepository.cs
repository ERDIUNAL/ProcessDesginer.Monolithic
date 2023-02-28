using Application.Services.Core.Repositories;
using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories.Core;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, BaseDbContext>, IUserOperationClaimRepository
{
    public UserOperationClaimRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<ICollection<OperationClaim>> GetOperationClaimsByUserIdAsync(int userId)
    {
        return await Query().AsNoTracking()
                            .Where(x => x.UserId == userId)
                            .Include(x => x.OperationClaim)
                            .Select(x => new OperationClaim
                            {
                                Id = x.OperationClaimId,
                                Name = x.OperationClaim.Name
                            }).ToListAsync();
    }
}
