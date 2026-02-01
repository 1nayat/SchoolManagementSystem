using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using School.Application.Common.Auth;

namespace School.Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    public Guid UserId { get; }
    public Guid? SchoolId { get; }
    public bool IsSuperAdmin { get; }

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity!.IsAuthenticated)
            throw new UnauthorizedAccessException("User is not authenticated");

        // UserId (from JWT)
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)
                          ?? throw new UnauthorizedAccessException("UserId claim missing");

        UserId = Guid.Parse(userIdClaim.Value);

        // Role
        IsSuperAdmin = user.IsInRole("SuperAdmin");

        // SchoolId (only for non-superadmin)
        if (!IsSuperAdmin)
        {
            var schoolIdClaim = user.FindFirst("schoolId")
                ?? throw new UnauthorizedAccessException("SchoolId claim missing");

            SchoolId = Guid.Parse(schoolIdClaim.Value);
        }
    }
}
