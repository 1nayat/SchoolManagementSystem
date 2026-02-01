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

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    private bool HasHttpContext => _httpContextAccessor.HttpContext != null;

    private bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            if (!HasHttpContext)
                throw new InvalidOperationException("UserId accessed outside HTTP request");

            if (!IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            var claim = User!.FindFirst(ClaimTypes.NameIdentifier)
                        ?? throw new UnauthorizedAccessException("UserId claim missing");

            return Guid.Parse(claim.Value);
        }
    }

    public Guid? SchoolId
    {
        get
        {
            // No HTTP context → system execution (startup, migrations)
            if (!HasHttpContext)
                return null;

            // Not authenticated → no tenant
            if (!IsAuthenticated)
                return null;

            // SuperAdmin → no tenant
            if (IsSuperAdmin)
                return null;

            var claim = User!.FindFirst("SchoolId")
                        ?? throw new UnauthorizedAccessException("SchoolId claim missing");

            return Guid.Parse(claim.Value);
        }
    }

    public bool IsSuperAdmin
    {
        get
        {
            // No HTTP context → system context (treat as SuperAdmin)
            if (!HasHttpContext)
                return true;

            if (!IsAuthenticated)
                return false;

            return User!.IsInRole("SuperAdmin");
        }
    }
}
