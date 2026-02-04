namespace School.API.Dtos.Common;

public record MeResponse(
    Guid UserId,
    string Email,
    IEnumerable<string> Roles,
    Guid? SchoolId,
    bool IsOnboarded,
    bool IsSuperAdmin
);
