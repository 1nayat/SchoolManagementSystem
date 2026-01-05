using Microsoft.AspNetCore.Identity;

namespace School.Infrastructure.Security;

public static class PasswordHasherHelper
{
    private static readonly PasswordHasher<string> _hasher = new();

    public static string Hash(string password)
        => _hasher.HashPassword("seed", password);

    public static bool Verify(string hash, string password)
        => _hasher.VerifyHashedPassword("seed", hash, password)
           == PasswordVerificationResult.Success;
}
