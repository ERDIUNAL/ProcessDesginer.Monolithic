using Crea.Core.Persistence.Repositories;
using Crea.Core.Security.Entities;

namespace Application.Services.Core.Repositories;

public interface IUserRepository : IRepository<User>, IAsyncRepository<User>
{
}
