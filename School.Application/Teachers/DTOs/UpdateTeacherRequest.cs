namespace School.Application.Teachers.DTOs;

public class UpdateTeacherRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string TeacherType { get; set; } = null!;
}
