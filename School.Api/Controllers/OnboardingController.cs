using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Application.Common.Auth;
using School.Infrastructure.Persistence;
using School.Domain.Entities;

namespace School.API.Controllers;

[ApiController]
[Route("api/onboarding")]
[Authorize(Roles = Roles.SchoolAdmin)]
public class OnboardingController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;
    private readonly ITokenService _tokenService;

    public OnboardingController(
        AppDbContext context,
        ICurrentUser currentUser,
        ITokenService tokenService)
    {
        _context = context;
        _currentUser = currentUser;
        _tokenService = tokenService;
    }

    [HttpPost("school")]
    public async Task<IActionResult> CreateSchool(
        CreateSchoolRequest request)
    {
        if (_currentUser.SchoolId != null)
            return BadRequest("School already created");

        var school = new Domain.Entities.School
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Board = request.Board,
            Medium = request.Medium,
            IsActive = true
        };

        _context.Schools.Add(school);

        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstAsync(u => u.Id == _currentUser.UserId);

        user.SchoolId = school.Id;

        await _context.SaveChangesAsync();

        var newToken = _tokenService.GenerateToken(user);

        return Ok(new
        {
            Token = newToken,
            SchoolId = school.Id
        });
    }
}
