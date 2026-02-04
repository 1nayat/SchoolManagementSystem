using School.Application.Common.Auth;

namespace School.Infrastructure.Auth;

public class SystemCurrentUser : ICurrentUser
{
    public Guid UserId => Guid.Empty;

    public Guid? SchoolId => null;

    public bool IsSuperAdmin => true;
}
