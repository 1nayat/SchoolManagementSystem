using School.Application.Common.Auth;

namespace School.Infrastructure.Security;

public class DesignTimeCurrentUser : ICurrentUser
{
    public Guid UserId => Guid.Empty;

    public Guid? SchoolId => null;

    public bool IsSuperAdmin => true;
}
