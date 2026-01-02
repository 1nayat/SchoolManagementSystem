using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;
namespace School.Domain.Entities;

public class ClassSubjectTeacher : SoftDeleteEntity
{
    public Guid ClassId { get; set; }
    public Guid SectionId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid TeacherId { get; set; }

    public Class Class { get; set; }
    public Section Section { get; set; }
    public Subject Subject { get; set; }
    public Teacher Teacher { get; set; }
}

