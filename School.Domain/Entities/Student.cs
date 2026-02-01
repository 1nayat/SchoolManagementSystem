using School.Domain.Common;

namespace School.Domain.Entities;

public class Student : TenantEntity
{
    // 🔑 Multi-tenancy
    public Guid SchoolId { get; set; }
    public School School { get; set; } = null!;

    public string AdmissionNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;

    public ICollection<StudentGuardian> Guardians { get; set; } = new List<StudentGuardian>();
}
