using Crea.Core.Persistence.Repositories;

namespace Crea.Core.Security.Entities;

public class UserEmailAuthenticator : Entity
{
    public int UserId { get; set; }
    public string? Key { get; set; }
    public bool IsVerified { get; set; }

    public virtual User User { get; set; }
}
