namespace School.Application.Common.Auth;

public interface ICurrentUser
{
    Guid UserId { get; }
    Guid? SchoolId { get; }
    bool IsSuperAdmin { get; }
}
