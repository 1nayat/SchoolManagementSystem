using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.API.Dtos.Common;
using School.Application.Common.Auth;
using School.Infrastructure.Persistence;

namespace School.API.Controllers;

[ApiController]
[Route("api/me")]
[Authorize] 
public class MeController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public MeController(
        AppDbContext context,
        ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<MeResponse>> GetMe()
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstAsync(u => u.Id == _currentUser.UserId);

        var roles = user.UserRoles
            .Select(ur => ur.Role.Name)
            .ToList();

        var isSuperAdmin = roles.Contains(Roles.SuperAdmin);

        var isOnboarded =
            isSuperAdmin ||
            _currentUser.SchoolId != null;

        return Ok(new MeResponse(
            UserId: user.Id,
            Email: user.Email,
            Roles: roles,
            SchoolId: _currentUser.SchoolId,
            IsOnboarded: isOnboarded,
            IsSuperAdmin: isSuperAdmin
        ));
    }
}
