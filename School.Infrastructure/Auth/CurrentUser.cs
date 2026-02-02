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
            if (!HasHttpContext)
                return null;

            if (!IsAuthenticated)
                return null;

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
            if (!HasHttpContext)
                return true;

            if (!IsAuthenticated)
                return false;

            return User!.IsInRole("SuperAdmin");
        }
    }
}
