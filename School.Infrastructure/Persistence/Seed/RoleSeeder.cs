using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Seed;

public class RoleSeeder
{
    private readonly AppDbContext _context;

    public RoleSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedRolesAsync()
    {
        if (await _context.Roles.AnyAsync())
            return;

        var roles = new List<Role>
        {
            new() { Id = Guid.NewGuid(), Name = "SuperAdmin" },
            new() { Id = Guid.NewGuid(), Name = "SchoolAdmin" },
            new() { Id = Guid.NewGuid(), Name = "Teacher" },
            new() { Id = Guid.NewGuid(), Name = "Student" },
            new() { Id = Guid.NewGuid(), Name = "Parent" }
        };

        _context.Roles.AddRange(roles);
        await _context.SaveChangesAsync();
    }
}
