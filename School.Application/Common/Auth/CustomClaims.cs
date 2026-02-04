using System.Security.Claims;

namespace School.Application.Common.Auth;

public static class CustomClaims
{
    public const string UserId = ClaimTypes.NameIdentifier;
    public const string Role = ClaimTypes.Role;

    public const string SchoolId = "SchoolId";
}
