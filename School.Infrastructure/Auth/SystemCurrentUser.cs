using School.Application.Common.Auth;

namespace School.Infrastructure.Auth;

public class SystemCurrentUser : ICurrentUser
{
    public Guid UserId => Guid.Empty;
    public Guid? SchoolId => null;

    public IReadOnlyCollection<string> Roles =>
        new[] { Roles.SuperAdmin };

    public Guid? TeacherId => null;
    public Guid? StudentId => null;

}
