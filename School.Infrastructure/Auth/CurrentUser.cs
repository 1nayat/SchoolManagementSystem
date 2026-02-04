using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using School.Application.Common.Auth;

namespace School.Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal User =>
        _httpContextAccessor.HttpContext?.User
        ?? throw new UnauthorizedAccessException("No HTTP context");

    public Guid UserId =>
        Guid.Parse(
            User.FindFirstValue(CustomClaims.UserId)
            ?? throw new UnauthorizedAccessException("UserId claim missing")
        );

    public Guid? SchoolId
    {
        get
        {
            var value = User.FindFirstValue(CustomClaims.SchoolId);
            return value == null ? null : Guid.Parse(value);
        }
    }

    public bool IsSuperAdmin =>
        User.IsInRole(Roles.SuperAdmin);
}
