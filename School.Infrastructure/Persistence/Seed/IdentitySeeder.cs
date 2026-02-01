using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using School.Domain.Entities;
using School.Infrastructure.Persistence;
using School.Infrastructure.Security;

namespace School.Infrastructure.Persistence.Seed;

public class IdentitySeeder
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public IdentitySeeder(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task SeedAsync()
    {
        await SeedUserAsync("SuperAdmin", null);
        await SeedUserAsync("SchoolAdmin", GetSchoolId("SchoolAdmin"));
        await SeedUserAsync("Teacher", GetSchoolId("Teacher"));
        await SeedUserAsync("Student", GetSchoolId("Student"));
        await SeedUserAsync("Parent", GetSchoolId("Parent"));
    }

    private async Task SeedUserAsync(string roleName, Guid? schoolId)
    {
        var email = _configuration[$"SeedUsers:{roleName}:Email"];
        var password = _configuration[$"SeedUsers:{roleName}:Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new Exception($"Seed credentials missing for role: {roleName}");

        var role = await _context.Roles.FirstAsync(r => r.Name == roleName);

        var existingUser = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (existingUser != null)
        {
            bool updated = false;

            if (existingUser.SchoolId == null && schoolId != null)
            {
                existingUser.SchoolId = schoolId;
                updated = true;
            }

            if (!existingUser.UserRoles.Any(ur => ur.RoleId == role.Id))
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = existingUser.Id,
                    RoleId = role.Id
                });
                updated = true;
            }

            if (updated)
                await _context.SaveChangesAsync();

            return;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = PasswordHasherHelper.Hash(password),
            SchoolId = schoolId,
            IsActive = true
        };

        _context.Users.Add(user);

        _context.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });

        await _context.SaveChangesAsync();
    }

    private Guid? GetSchoolId(string roleName)
    {
        var value = _configuration[$"SeedUsers:{roleName}:SchoolId"];
        return string.IsNullOrWhiteSpace(value)
            ? null
            : Guid.Parse(value);
    }
}
