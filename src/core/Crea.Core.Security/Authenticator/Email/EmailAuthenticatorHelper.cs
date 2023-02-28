using System.Security.Cryptography;

namespace Crea.Core.Security.Authenticator.Email;

public class EmailAuthenticatorHelper : IEmailAuthenticatorHelper
{
    public Task<string> CreateEmailActivationKeyAsync()
    {
        byte[] key = RandomNumberGenerator.GetBytes(64);

        return Task.FromResult(Convert.ToBase64String(key));
    }

    public Task<string> CreateEmailAuthenticatorCodeAsync()
    {
        var code = RandomNumberGenerator.GetInt32(Convert.ToInt32(Math.Pow(10, 6))).ToString().PadLeft(6, '0');

        return Task.FromResult(code);
    }
}
