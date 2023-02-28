using Crea.Core.Security.Entities;

namespace Crea.Core.Security.JWT;

public interface ITokenHelper
{
    public int RefreshTokenTTLOption { get; }
    public AccessToken CreateToken(User user, ICollection<OperationClaim> operationClaims);
    public RefreshToken CreateRefreshToken(User user, string ipAddress);
}
