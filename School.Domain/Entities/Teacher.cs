using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class Teacher : TenantEntity
{
    public string EmployeeCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string TeacherType { get; set; } // Full-Time, Part-Time
}

