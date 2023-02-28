using Application.Services.Core.Repositories;
using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;
using Persistence.Contexts;

namespace Persistence.Repositories.Core;

public class OperationClaimRepository : EfRepositoryBase<OperationClaim, BaseDbContext>, IOperationClaimRepository
{
    public OperationClaimRepository(BaseDbContext context) : base(context)
    {
    }
}
