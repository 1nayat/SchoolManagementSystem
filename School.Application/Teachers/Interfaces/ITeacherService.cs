using School.Application.Teachers.DTOs;

namespace School.Application.Teachers.Interfaces;

public interface ITeacherService
{
    Task<List<TeacherListItem>> GetAllAsync();
    Task<TeacherDetailsDto> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, UpdateTeacherRequest request);
    Task UpdateStatusAsync(Guid id, bool isActive);
}
