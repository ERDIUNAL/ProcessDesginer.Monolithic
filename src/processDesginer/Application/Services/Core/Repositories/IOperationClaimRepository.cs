using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;

namespace Application.Services.Core.Repositories;

public interface IOperationClaimRepository : IRepository<OperationClaim>, IAsyncRepository<OperationClaim>
{
}
