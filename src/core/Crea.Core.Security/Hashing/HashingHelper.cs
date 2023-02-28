using System.Security.Cryptography;
using System.Text;

namespace Crea.Core.Security.Hashing;

public static class HashingHelper
{
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new(passwordSalt);

        passwordSalt = hmac.Key;
        byte[] computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computeHash.SequenceEqual(passwordHash);
    }
}
