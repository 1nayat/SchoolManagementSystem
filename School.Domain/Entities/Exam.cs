using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using School.Domain.Common;

namespace School.Domain.Entities;

public class Exam : SoftDeleteEntity
{
    public string ExamName { get; set; }
    public string ExamType { get; set; } 
    public DateTime ExamDate { get; set; }
}
