namespace School.Application.Teachers.DTOs;

public class InviteTeacherRequest
{
    public string Email { get; set; } = null!;
    public string EmployeeCode { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string TeacherType { get; set; } = null!;
}
