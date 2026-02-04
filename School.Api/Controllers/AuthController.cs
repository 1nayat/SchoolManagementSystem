using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Api.Dtos.Auth;
using School.API.Dtos.Auth;
using School.Application.Common.Auth;
using School.Domain.Entities;
using School.Infrastructure.Persistence;
using School.Infrastructure.Security;

namespace School.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(
        AppDbContext context,
        ITokenService tokenService)
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
            .FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                !u.IsDeleted &&
                u.IsActive);

        if (user == null)
            return Unauthorized("Invalid credentials");

        if (!PasswordHasherHelper.Verify(
                user.PasswordHash,
                request.Password))
        {
            return Unauthorized("Invalid credentials");
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new LoginResponse(
            token,
            user.Email
        ));
    }


    [HttpPost("register-school-admin")]
    public async Task<IActionResult> RegisterSchoolAdmin(
        RegisterSchoolAdminRequest request)
    {
        var exists = await _context.Users
            .AnyAsync(u => u.Email == request.Email);

        if (exists)
            return BadRequest("User already exists");

        var role = await _context.Roles
            .FirstAsync(r => r.Name == Roles.SchoolAdmin);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = PasswordHasherHelper.Hash(request.Password),
            SchoolId = null,          
            IsActive = true
        };

        _context.Users.Add(user);

        _context.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "SchoolAdmin registered successfully. Please login to continue onboarding."
        });
    }
}
