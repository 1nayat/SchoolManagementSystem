using Microsoft.EntityFrameworkCore;
using School.Application.Common.Auth;
using School.Application.Students.DTOs;
using School.Application.Students.Interfaces;
using School.Infrastructure.Persistence;

public class StudentService : IStudentService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _currentUser;

    public StudentService(AppDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Guid> UpsertAsync(UpsertStudentRequest request)
    {
        var schoolId = _currentUser.SchoolId
            ?? throw new UnauthorizedAccessException();

        if (_currentUser.Roles.Contains(Roles.Teacher)
)
        {
            var allowed = await _db.ClassSubjectTeachers
                .AnyAsync(x =>
                    x.TeacherId == _currentUser.TeacherId &&
                    x.ClassId == request.ClassId &&
                    x.SectionId == request.SectionId);

            if (!allowed)
                throw new UnauthorizedAccessException(
                    "Teacher not assigned to this class/section");
        }

        Student student;

        if (request.Id.HasValue)
        {
            student = await _db.Students
                .FirstOrDefaultAsync(x => x.Id == request.Id.Value)
                ?? throw new KeyNotFoundException("Student not found");

            if (student.SchoolId != schoolId)
                throw new UnauthorizedAccessException();
        }
        else
        {
            student = new Student
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId
            };

            _db.Students.Add(student);
        }

        student.AdmissionNumber = request.AdmissionNumber;
        student.DateOfBirth = request.DateOfBirth;
        student.Gender = request.Gender;
        student.ClassId = request.ClassId;
        student.SectionId = request.SectionId;

        await _db.SaveChangesAsync();

        return student.Id;
    }
        public async Task<List<StudentListItem>> GetAllAsync()
    {
        var schoolId = _currentUser.SchoolId
            ?? throw new UnauthorizedAccessException();

        var query = _db.Students
            .Include(x => x.Class)
            .Include(x => x.Section)
            .Where(x => x.SchoolId == schoolId);

        if (_currentUser.Roles.Contains(Roles.Teacher))
        {
            var allowedClassSections = _db.ClassSubjectTeachers
                .Where(x => x.TeacherId == _currentUser.TeacherId)
                .Select(x => new { x.ClassId, x.SectionId });

            query = query.Where(s =>
                allowedClassSections.Any(a =>
                    a.ClassId == s.ClassId &&
                    a.SectionId == s.SectionId));
        }

        return await query
            .Select(s => new StudentListItem
            {
                Id = s.Id,
                AdmissionNumber = s.AdmissionNumber,
                FullName = s.AdmissionNumber, 
                ClassSection = s.Class.ClassName + "-" + s.Section.SectionName,
                IsActive = s.IsActive
            })
            .ToListAsync();
    }

    public async Task<StudentDetailsDto> GetByIdAsync(Guid id)
    {
        var schoolId = _currentUser.SchoolId
            ?? throw new UnauthorizedAccessException();

        var student = await _db.Students
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new KeyNotFoundException("Student not found");

        if (student.SchoolId != schoolId)
            throw new UnauthorizedAccessException();

        if (_currentUser.Roles.Contains(Roles.Teacher))
        {
            var allowed = await _db.ClassSubjectTeachers.AnyAsync(x =>
                x.TeacherId == _currentUser.TeacherId &&
                x.ClassId == student.ClassId &&
                x.SectionId == student.SectionId);

            if (!allowed)
                throw new UnauthorizedAccessException();
        }

        return new StudentDetailsDto
        {
            Id = student.Id,
            AdmissionNumber = student.AdmissionNumber,
            DateOfBirth = student.DateOfBirth,
            Gender = student.Gender,
            ClassId = student.ClassId,
            SectionId = student.SectionId,
            IsActive = student.IsActive
        };
    }


    public async Task UpdateStatusAsync(Guid id, bool isActive)
    {
        var schoolId = _currentUser.SchoolId
            ?? throw new UnauthorizedAccessException();

        var student = await _db.Students
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new KeyNotFoundException("Student not found");

        if (student.SchoolId != schoolId)
            throw new UnauthorizedAccessException();

        if (!_currentUser.Roles.Contains(Roles.SchoolAdmin))
            throw new UnauthorizedAccessException("Only admin can change status");

        student.IsActive = isActive;
        await _db.SaveChangesAsync();
    }

}

