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

    public async Task SeedSuperAdminAsync()
    {
        if (await _context.Users.AnyAsync(u =>
            u.UserRoles.Any(r => r.Role.Name == "SuperAdmin")))
            return;

        var email = _configuration["SeedUsers:SuperAdmin:Email"];
        var password = _configuration["SeedUsers:SuperAdmin:Password"];

        var superAdminRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");

        if (superAdminRole == null)
            throw new Exception("SuperAdmin role not found. Ensure roles are seeded first.");

        var passwordHash = PasswordHasherHelper.Hash(password!);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email!,
            PasswordHash = passwordHash,
            SchoolId = null, 
            IsActive = true,
            UserRoles = new List<UserRole>()
        };

        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,              
            RoleId = superAdminRole.Id
        });

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

    }
}
