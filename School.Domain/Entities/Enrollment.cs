using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class Enrollment : SoftDeleteEntity
{
    public Guid StudentId { get; set; }
    public Guid ClassId { get; set; }
    public Guid SectionId { get; set; }

    public int AcademicYear { get; set; }
    public DateTime EnrollmentDate { get; set; }

    public Student Student { get; set; }
    public Class Class { get; set; }
    public Section Section { get; set; }
}
