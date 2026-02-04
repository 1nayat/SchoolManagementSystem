namespace School.API.Dtos.Auth;

public record LoginRequest(
    string Email,
    string Password
);
