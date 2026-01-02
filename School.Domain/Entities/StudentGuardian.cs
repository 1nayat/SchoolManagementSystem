using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class StudentGuardian : SoftDeleteEntity
{
    public Guid StudentId { get; set; }
    public Guid GuardianId { get; set; }
    public string Relationship { get; set; }

    public Student Student { get; set; }
    public Guardian Guardian { get; set; }
}

