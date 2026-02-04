namespace School.API.Dtos.Auth;

public record LoginResponse(
    string Token,
    string Email
);
