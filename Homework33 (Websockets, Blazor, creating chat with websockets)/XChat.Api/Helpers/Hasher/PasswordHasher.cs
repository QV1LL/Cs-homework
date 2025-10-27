using System.Security.Cryptography;
using System.Text;

namespace XChat.Api.Helpers.Hasher;

internal static class PasswordHasher
{
    public static string Encrypt(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public static bool Verify(string password, string hashedPassword)
    {
        var hashToCheck = Encrypt(password);
        return hashToCheck == hashedPassword;
    }
}