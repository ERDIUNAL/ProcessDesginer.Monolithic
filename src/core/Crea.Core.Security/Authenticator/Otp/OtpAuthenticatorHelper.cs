using OtpNet;

namespace Crea.Core.Security.Authenticator.Otp;

public class OtpAuthenticatorHelper : IOtpAuthenticatorHelper
{
    //20 byte uzunluğu. 20 olunca 6 haneli kod üretimi için uygundur
    public Task<byte[]> GenerateSecretKeyAsync() => Task.FromResult(KeyGeneration.GenerateRandomKey(20));
    
    public Task<string> ConvertSecretKeyToStringAsync(byte[] secretKeyBytes) => Task.FromResult(Base32Encoding.ToString(secretKeyBytes));

    public Task<bool> VerifyCodeAsync(byte[] secretKeyBytes, string code)
    {
        Totp totp = new(secretKeyBytes);

        string generatedOtpCode = totp.ComputeTotp(DateTime.UtcNow); //verilen zamanda üretilen zaman bazlı kodu üretir.

        bool result = generatedOtpCode.Equals(code);

        return Task.FromResult(result);
    }
}
