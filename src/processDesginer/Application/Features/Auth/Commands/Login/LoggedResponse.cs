using Crea.Core.Security.Authenticator;
using Crea.Core.Security.Entities;
using Crea.Core.Security.JWT;

namespace Application.Features.Auth.Commands.Login;

public class LoggedResponse
{
    public AccessToken? AccessToken { get; set; }
    public RefreshToken? RefreshToken { get; set; }
    public AuthenticatorType? RequiredAuthenticatorType { get; set; }

    public class LoggedHttpResponse
    {
        public AccessToken? AccessToken { get; set; }
        public AuthenticatorType? RequiredAuthenticatorType { get; set; }
    }

    public LoggedHttpResponse ToHttpResponse() =>
        new()
        {
            AccessToken = this.AccessToken,
            RequiredAuthenticatorType = this.RequiredAuthenticatorType
        };
}
