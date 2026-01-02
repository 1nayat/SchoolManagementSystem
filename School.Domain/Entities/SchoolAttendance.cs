using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class StudentAttendance : SoftDeleteEntity
{
    public Guid EnrollmentId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public string Status { get; set; } 

    public Enrollment Enrollment { get; set; }
}

