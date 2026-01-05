using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Auth;
using School.Infrastructure.Persistence;
using School.Infrastructure.Security;

namespace School.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);

        if (user == null)
            return Unauthorized("Invalid credentials");

        if (!PasswordHasherHelper.Verify(user.PasswordHash, request.Password))
            return Unauthorized("Invalid credentials");

        var roles = user.UserRoles.Select(r => r.Role.Name);

        var token = _tokenService.GenerateToken(user, roles);

        return Ok(new
        {
            token,
            user.Id,
            user.Email,
            roles
        });
    }
}

public record LoginRequest(string Email, string Password);
