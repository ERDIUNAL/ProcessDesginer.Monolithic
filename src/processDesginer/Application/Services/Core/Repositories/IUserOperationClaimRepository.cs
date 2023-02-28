using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;

namespace Application.Services.Core.Repositories;

public interface IUserOperationClaimRepository : IRepository<UserOperationClaim>, IAsyncRepository<UserOperationClaim>
{
    public Task<ICollection<OperationClaim>> GetOperationClaimsByUserIdAsync(int userId);
}
