using Microsoft.AspNetCore.Http;
using School.Application.Common.Auth;
using System.Security.Claims;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public Guid UserId
    {
        get
        {
            var value = User?.FindFirstValue(CustomClaims.UserId);
            return value == null ? Guid.Empty : Guid.Parse(value);
        }
    }

    public Guid? SchoolId
    {
        get
        {
            var value = User?.FindFirstValue(CustomClaims.SchoolId);
            return value == null ? null : Guid.Parse(value);
        }
    }

    public IReadOnlyCollection<string> Roles =>
      User?
          .FindAll(ClaimTypes.Role)
          .Select(c => c.Value)
          .ToList()
      ?? Enumerable.Empty<string>().ToList();


    public Guid? TeacherId
    {
        get
        {
            var value = User?.FindFirstValue(CustomClaims.TeacherId);
            return value == null ? null : Guid.Parse(value);
        }
    }

    public Guid? StudentId
    {
        get
        {
            var value = User?.FindFirstValue(CustomClaims.StudentId);
            return value == null ? null : Guid.Parse(value);
        }
    }

    public bool IsSuperAdmin =>
        Roles.Contains(RolesConstants.SuperAdmin);
}
