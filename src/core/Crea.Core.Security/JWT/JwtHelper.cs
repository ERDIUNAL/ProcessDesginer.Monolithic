using Crea.Core.Security.Entities;
using Crea.Core.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Crea.Core.Security.JWT;

public class JwtHelper : ITokenHelper
{
    private TokenOptions _tokenOptions;
    private DateTime _accessTokenExpiration;

    public JwtHelper(IConfiguration configuration)
    {
        _tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>()!;
    }

    public int RefreshTokenTTLOption => _tokenOptions.RefreshTokenTTL;

    public AccessToken CreateToken(User user, ICollection<OperationClaim> operationClaims)
    {
        _accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration);

        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
        var jwt = createJwtSecurityToken(user, operationClaims, signingCredentials);

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

        string token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new AccessToken
        {
            Token = token,
            Expiration = _accessTokenExpiration
        };
    }

    public RefreshToken CreateRefreshToken(User user, string ipAddress)
    {
        return new RefreshToken
        {
            UserId = user.Id,
            Token = generateRandomRefreshToken(),
            CretedByIp = ipAddress,
            ExpiresDate = DateTime.UtcNow.AddMinutes(_tokenOptions.RefreshTokenExpiration)
        };
    }

    private JwtSecurityToken createJwtSecurityToken(User user, ICollection<OperationClaim> operationClaims, SigningCredentials signingCredentials)
    {
        return new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            expires: _accessTokenExpiration,
            notBefore: DateTime.UtcNow,
            claims: SetClaims(user, operationClaims),
            signingCredentials: signingCredentials);
    }

    private IEnumerable<Claim> SetClaims(User user, ICollection<OperationClaim> operationClaims)
    {
        List<Claim> claims = new();

        claims.AddNameIdentifier(user.Id.ToString());
        claims.AddEmail(user.Email);
        claims.AddName($"{user.FirstName} {user.LastName}");
        claims.AddRoles(operationClaims.Select(x => x.Name).ToArray());

        return claims.ToArray();
    }

    private string generateRandomRefreshToken()
    {
        byte[] bytes = new byte[32];

        using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
