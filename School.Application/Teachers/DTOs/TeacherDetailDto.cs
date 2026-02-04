namespace School.Application.Teachers.DTOs;

public class TeacherDetailsDto
{
    public Guid Id { get; set; }
    public string EmployeeCode { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string TeacherType { get; set; } = null!;
    public bool IsProfileCompleted { get; set; }
    public bool IsActive { get; set; }

    public string Email { get; set; } = null!;
}
