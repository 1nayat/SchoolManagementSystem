namespace School.Application.Teachers.DTOs;

public class TeacherListItem
{
    public Guid Id { get; set; }
    public string EmployeeCode { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string TeacherType { get; set; } = null!;
    public bool IsActive { get; set; }
    public bool IsProfileCompleted { get; set; }
}
