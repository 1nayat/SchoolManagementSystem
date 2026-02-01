using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Infrastructure.Persistence;

namespace School.Infrastructure.Persistence.Seed;

public class SchoolSeeder
{
    private readonly AppDbContext _context;

    public SchoolSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Idempotent
        if (await _context.Schools.AnyAsync())
            return;

        var school = new School.Domain.Entities.School
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Demo Public School",
            Board = "CBSE",
            Medium = "English",
            IsActive = true
        };

        _context.Schools.Add(school);
        await _context.SaveChangesAsync();
    }
}
