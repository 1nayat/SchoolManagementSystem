namespace School.Application.Common.Auth;

public static class Policies
{
    public const string RequireSuperAdmin = "RequireSuperAdmin";
    public const string RequireSchoolAdmin = "RequireSchoolAdmin";
    public const string RequireOnboardedSchoolAdmin = "RequireOnboardedSchoolAdmin";
}
