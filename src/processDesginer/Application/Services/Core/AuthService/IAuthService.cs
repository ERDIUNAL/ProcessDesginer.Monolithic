using Crea.Core.Security.Entities;
using Crea.Core.Security.JWT;

namespace Application.Services.Core.AuthService;

public interface IAuthService
{
    public Task<AccessToken> CreateAccessToken(User user);
    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress);
    public Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
    public Task DeleteOldActiveRefreshTokens(User user);

    public Task RevokeRefreshToken(RefreshToken token, string ipAddress, string reason, string? replacedByToken = default);
    public Task RevokeDescendantRefreshTokens(RefreshToken token, string ipAddress, string reason);

    public Task<RefreshToken> RotateRefreshToken(User user, RefreshToken token, string ipAddress);

    public Task<UserEmailAuthenticator> CreateEmailAuthenticator(User user);
    public Task<UserOtpAuthenticator> CreateOtpAuthenticator(User user);

    public Task<string> ConvertOtpSecretKeyToString(byte[] secretKeyBytes);

    public Task SendAuthenticatorCode(User user);
    public Task VerifyAuthenticatorCode(User user, string code);
}
