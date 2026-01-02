using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class ExamResult : SoftDeleteEntity
{
    public Guid ExamId { get; set; }
    public Guid EnrollmentId { get; set; }
    public Guid SubjectId { get; set; }

    public decimal Marks { get; set; }

    public Exam Exam { get; set; }
    public Enrollment Enrollment { get; set; }
    public Subject Subject { get; set; }
}
