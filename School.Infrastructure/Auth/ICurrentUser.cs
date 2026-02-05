namespace School.Application.Common.Auth;

public interface ICurrentUser
{
    Guid UserId { get; }
    Guid? SchoolId { get; }
    IReadOnlyCollection<string> Roles { get; }
    Guid? TeacherId { get; }
    Guid? StudentId { get; }
    bool IsSuperAdmin { get; }
}
