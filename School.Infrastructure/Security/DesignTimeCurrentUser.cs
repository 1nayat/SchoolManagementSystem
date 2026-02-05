using School.Application.Common.Auth;

namespace School.Infrastructure.Auth;

public class DesignTimeCurrentUser : ICurrentUser
{
    public Guid UserId => Guid.Empty;
    public Guid? SchoolId => Guid.Empty;

    public IReadOnlyCollection<string> Roles =>
        new[] { Roles.IsSuperAdmin };

    public Guid? TeacherId => null;
    public Guid? StudentId => null;

    public bool IsSuperAdmin => throw new NotImplementedException();
}
