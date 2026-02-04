namespace School.API.Dtos.Onboarding;

public record CreateSchoolRequest(
    string Name,
    string Board,
    string Medium
);
