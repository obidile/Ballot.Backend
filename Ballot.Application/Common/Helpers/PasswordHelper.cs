using System.Security.Cryptography;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Ballot.Application.Common.Helpers;

public class PasswordHelper
{
    public static string HashPassword(string password, string salt)
    {
        return BCryptNet.HashPassword(password + salt);
    }

    public static bool VerifyPassword(string password, string salt, string hash)
    {
        return BCryptNet.Verify(password + salt, hash);
    }

    public static string GenerateSalt()
    {
        var salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        return Convert.ToBase64String(salt);
    }
}
