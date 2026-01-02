using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class Subject : SoftDeleteEntity
{
    public string SubjectName { get; set; }
    public string SubjectCode { get; set; }
}

