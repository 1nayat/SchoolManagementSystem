using Microsoft.EntityFrameworkCore;
using School.Application.Common.Auth;
using School.Application.Teachers.DTOs;
using School.Application.Teachers.Interfaces;
using School.Infrastructure.Persistence;

public class TeacherService : ITeacherService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _currentUser;

    public TeacherService(AppDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<TeacherListItem>> GetAllAsync()
    {
        var schoolId = _currentUser.SchoolId
            ?? throw new UnauthorizedAccessException();

        return await _db.Teachers
            .Where(t => t.SchoolId == schoolId)
            .Select(t => new TeacherListItem
            {
                Id = t.Id,
                EmployeeCode = t.EmployeeCode,
                FullName = t.FirstName + " " + t.LastName,
                TeacherType = t.TeacherType,
                IsProfileCompleted = t.IsProfileCompleted,
                IsActive = t.User != null && t.User.IsActive
            })
            .ToListAsync();
    }

    public async Task<TeacherDetailsDto> GetByIdAsync(Guid id)
    {
        var schoolId = _currentUser.SchoolId;

        var teacher = await _db.Teachers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teacher == null)
            throw new KeyNotFoundException("Teacher not found");

        if (teacher.SchoolId != schoolId)
            throw new UnauthorizedAccessException();

        return new TeacherDetailsDto
        {
            Id = teacher.Id,
            EmployeeCode = teacher.EmployeeCode,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            TeacherType = teacher.TeacherType,
            IsProfileCompleted = teacher.IsProfileCompleted,
            IsActive = teacher.User?.IsActive ?? false,
            Email = teacher.User?.Email ?? "Not onboarded yet"
        };
    }

    public async Task UpdateAsync(Guid id, UpdateTeacherRequest request)
    {
        var teacher = await _db.Teachers.FindAsync(id)
            ?? throw new KeyNotFoundException();

        if (teacher.SchoolId != _currentUser.SchoolId)
            throw new UnauthorizedAccessException();

        teacher.FirstName = request.FirstName;
        teacher.LastName = request.LastName;
        teacher.TeacherType = request.TeacherType;

        await _db.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(Guid id, bool isActive)
    {
        var teacher = await _db.Teachers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id)
            ?? throw new KeyNotFoundException();

        if (teacher.SchoolId != _currentUser.SchoolId)
            throw new UnauthorizedAccessException();

        if (teacher.User == null)
            throw new InvalidOperationException("Teacher not onboarded yet");

        teacher.User.IsActive = isActive;
        await _db.SaveChangesAsync();
    }
}
