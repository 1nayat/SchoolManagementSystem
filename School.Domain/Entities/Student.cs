using School.Domain.Common;
using School.Domain.Entities;

public class Student : TenantEntity
{
    public School.Domain.Entities.School School { get; set; } = null!;

    public string AdmissionNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;

    public Guid ClassId { get; set; }
    public Guid SectionId { get; set; }

    public Class Class { get; set; } = null!;
    public Section Section { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public ICollection<StudentGuardian> Guardians { get; set; }
        = new List<StudentGuardian>();
}
